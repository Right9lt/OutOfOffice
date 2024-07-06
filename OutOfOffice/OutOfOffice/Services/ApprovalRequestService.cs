using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Context;
using OutOfOffice.DTO.ApprovalRequest;
using OutOfOffice.Entities;

namespace OutOfOffice.Services
{
    public class ApprovalRequestService(OutOfOfficeContext context, IMapper mapper)
    {
        private readonly OutOfOfficeContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ApprovalRequestOutDTO>> GetAll(int role, int employeeId)
        {
            return role switch
            {
                0=> await _context.ApprovalRequests.Include(a=>a.Approver).Include(a => a.LeaveRequest).ThenInclude(l => l.Employee).ThenInclude(e=>e.PeoplePartner).Where(a => (a.LeaveRequest.Employee.PeoplePartner != null && a.LeaveRequest.Employee.PeoplePartner.Id == employeeId) || a.LeaveRequest.Employee.Id == employeeId).ProjectTo<ApprovalRequestOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),

                1 => await _context.ApprovalRequests.Include(a => a.Approver).Include(a => a.LeaveRequest).ThenInclude(l => l.Employee).ThenInclude(e => e.Projects).Where(a=>a.LeaveRequest.Employee.Projects.Any(p => _context.Projects.Include(e => e.Employees).Any(p1 => p1.Employees.Any(e => e.Id == employeeId) && p1.Id == p.Id)) || a.LeaveRequest.Employee.Id == employeeId).ProjectTo<ApprovalRequestOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),

                2 => await _context.ApprovalRequests.Include(a => a.Approver).Include(a => a.LeaveRequest).ThenInclude(l => l.Employee).ProjectTo<ApprovalRequestOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),

                _ => await _context.ApprovalRequests.Include(a => a.Approver).Include(a => a.LeaveRequest).ThenInclude(l => l.Employee).Where(a => a.LeaveRequest.Employee.Id == employeeId).ProjectTo<ApprovalRequestOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),
            };
        }

        private async Task<ApprovalRequest?> GetApprovalRequest(int id, int role, int employeeId)
        {
            return role switch
            {
                0 => await _context.ApprovalRequests.Include(a => a.Approver).Include(a => a.LeaveRequest).ThenInclude(l => l.Employee).ThenInclude(e => e.PeoplePartner).FirstOrDefaultAsync(a => ((a.LeaveRequest.Employee.PeoplePartner != null && a.LeaveRequest.Employee.PeoplePartner.Id == employeeId) || a.LeaveRequest.Employee.Id == employeeId) && a.Id == id),

                1 => await _context.ApprovalRequests.Include(a => a.Approver).Include(a => a.LeaveRequest).ThenInclude(l => l.Employee).ThenInclude(e => e.Projects).FirstOrDefaultAsync(a =>(a.LeaveRequest.Employee.Projects.Any(p => _context.Projects.Include(e => e.Employees).Any(p1 => p1.Employees.Any(e => e.Id == employeeId) && p1.Id == p.Id)) || a.LeaveRequest.Employee.Id == employeeId) && a.Id == id),

                2 => await _context.ApprovalRequests.Include(a => a.Approver).Include(a => a.LeaveRequest).ThenInclude(l => l.Employee).FirstOrDefaultAsync(a=>a.Id ==id),

                _ => await _context.ApprovalRequests.Include(a => a.Approver).Include(a => a.LeaveRequest).ThenInclude(l => l.Employee).FirstOrDefaultAsync(a => a.Id == id && a.LeaveRequest.Employee.Id == employeeId),
            };
        }

        public async Task<Result<ApprovalRequestOutDTO>> GetById(int id, int role, int employeeId)
        {
            var approvalRequest = await GetApprovalRequest(id, role, employeeId);

            if (approvalRequest == null)
            {
                return Result.Failure<ApprovalRequestOutDTO>("Approval request was not found");
            }

            return Result.Success(_mapper.Map<ApprovalRequestOutDTO>(approvalRequest));
        }
        //0-approved 1=declined 2- canceled, 3 -new 
        public async Task<Result> Add(ApprovalRequestDTO model, int employeeId)
        {
            var approver = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (approver == null)
            {
                return Result.Failure("Can't approve this request from this account");
            }

            var leaveRequest = await _context.LeaveRequests.Include(l => l.Employee).FirstOrDefaultAsync(l => l.Id == model.LeaveRequestId);

            if (leaveRequest == null)
            {
                return Result.Failure("Leave request was not found");
            }


            var approvalRequest = new ApprovalRequest()
            {
                Approver = approver,
                LeaveRequest = leaveRequest,
                Status = model.Status,
                Comment = model.Comment,
            };

            await _context.AddAsync(approvalRequest);

            leaveRequest.Status = model.Status;


            if (model.Status == 0)
            {
                var diff = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
                leaveRequest.Employee.OutOfOfficeBalance = leaveRequest.Employee.OutOfOfficeBalance - diff;
            }
            _context.Update(leaveRequest);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
