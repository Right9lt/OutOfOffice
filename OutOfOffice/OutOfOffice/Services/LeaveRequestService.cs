using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Context;
using OutOfOffice.DTO.Employee;
using OutOfOffice.DTO.LeaveRequest;
using OutOfOffice.Entities;
using System.Data;

namespace OutOfOffice.Services
{
    public class LeaveRequestService(OutOfOfficeContext context, IMapper mapper)
    {
        private readonly OutOfOfficeContext _context = context;
        private readonly IMapper _mapper = mapper;


        public async Task<List<LeaveRequestOutDTO>> GetAll(int role, int employeeId)
        {
            return role switch
            {
                0 => await _context.LeaveRequests.Include(l => l.Employee).ThenInclude(l => l.PeoplePartner).Where(l => (l.Employee.PeoplePartner != null && l.Employee.PeoplePartner.Id == employeeId) || l.Employee.Id == employeeId).ProjectTo<LeaveRequestOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),

                1 => await _context.LeaveRequests.Include(l => l.Employee).ThenInclude(e => e.Projects)
                .Where(l => l.Employee.Projects.Any(p => _context.Projects.Include(e => e.Employees).Any(p1 => p1.Employees.Any(e => e.Id == employeeId) && p1.Id == p.Id)) || l.Employee.Id == employeeId).ProjectTo<LeaveRequestOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),

                2 => await _context.LeaveRequests.Include(l => l.Employee).ProjectTo<LeaveRequestOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),

                _ => await _context.LeaveRequests.Include(l => l.Employee).Where(l => l.Employee.Id == employeeId).ProjectTo<LeaveRequestOutDTO>(_mapper.ConfigurationProvider).ToListAsync()
            };
        }
        /// <summary>
        /// Searches for leave request by id. The method takes into account the person who is searching, to prevent data from appearing in the wrong hands
        /// </summary>
        /// <param name="id">Id of searched leave request</param>
        /// <param name="role">Role of searcher</param>
        /// <param name="employeeId">Id of searcher</param>
        /// <returns>If leave request is exist and searcher has permission to have this data, method returns the employee. Otherwise, method returns NULL</returns>
        private async Task<LeaveRequest?> GetLeaveRequest(int id, int role, int employeeId)
        {
            return role switch
            {
                0 => await _context.LeaveRequests.Include(l => l.ApprovalRequest).Include(l => l.Employee).ThenInclude(l => l.PeoplePartner).FirstOrDefaultAsync(l => l.Id == id && ((l.Employee.PeoplePartner != null && l.Employee.PeoplePartner.Id == employeeId) || l.Employee.Id == employeeId)),

                1 => await _context.LeaveRequests.Include(l => l.ApprovalRequest).Include(l => l.Employee).ThenInclude(e => e.Projects)
                .FirstOrDefaultAsync(l => l.Id == id && (l.Employee.Projects.Any(p => _context.Projects.Include(e => e.Employees).Any(p1 => p1.Employees.Any(e => e.Id == employeeId) && p1.Id == p.Id)) || l.Employee.Id == employeeId)),

                2 => await _context.LeaveRequests.Include(l => l.ApprovalRequest).Include(l => l.Employee).FirstOrDefaultAsync(l => l.Id == id),

                _ => await _context.LeaveRequests.Include(l => l.ApprovalRequest).Include(l => l.Employee).FirstOrDefaultAsync(l => l.Employee.Id == employeeId && l.Id == id)
            };
        }

        public async Task<Result<LeaveRequestOutDTO>> GetById(int id, int role, int employeeId)
        {
            var request = await GetLeaveRequest(id, role, employeeId);

            if (request == null)
            {
                return Result.Failure<LeaveRequestOutDTO>("Leave request was not found");
            }

            return Result.Success(_mapper.Map<LeaveRequestOutDTO>(request));
        }

        public async Task<Result> Add(LeaveRequestDTO model, int employeeId)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null)
            {
                return Result.Failure("Employee was not found");
            }

            var diff = model.EndDate.DayNumber - model.StartDate.DayNumber;

            if (diff > employee.OutOfOfficeBalance)
            {
                return Result.Failure($"Requested leave days count is {diff}, which is bigger than your out of office balance ({employee.OutOfOfficeBalance}). Change leave dates");
            }

            var request = new LeaveRequest()
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Comment = model.Comment,
                AbsenseReason = model.AbsenseReason,

                Employee = employee,
                Status = 3,
            };

            await _context.AddAsync(request);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        //0-approved 1=declined 2- canceled, 3 -new 
        public async Task<Result> Cancel(int id, int role, int employeeId)
        {
            var request = await GetLeaveRequest(id, role, employeeId);


            if (request == null)
            {
                return Result.Failure("Leave request was not found");
            }

            if (request.ApprovalRequest != null)
            {
                request.ApprovalRequest.Status = 2;

                if (request.ApprovalRequest.Status == 0)
                {
                    var diff = request.EndDate.DayNumber - request.StartDate.DayNumber;
                    request.Employee.OutOfOfficeBalance = request.Employee.OutOfOfficeBalance + diff;
                }
            }

            request.Status = 2;

            _context.Update(request);

            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<List<SelectListItem>> GetNewLeaveRequestsSelectionList(int role, int employeeId)
        {
            var selection = new List<SelectListItem>();

            var requests = role switch
            {
                0 => _context.LeaveRequests.Include(l => l.Employee).ThenInclude(e => e.PeoplePartner).Where(l => l.Status == 3 && l.Employee.PeoplePartner.Id == employeeId && l.Employee.Id != employeeId),
                1 => _context.LeaveRequests.Include(l => l.Employee).ThenInclude(e => e.Projects).Where(l => l.Status == 3 && l.Employee.Id != employeeId && l.Employee.Projects.Any(p => p.Employees.Any(pe => pe.Id == employeeId))),
                _ => _context.LeaveRequests.Include(l => l.Employee).Where(l=>l.Status == 3 && l.Employee.Id != employeeId)
            };

            await requests.ForEachAsync(l => selection.Add(new SelectListItem($"{l.Employee.Fullname} {l.StartDate}-{l.EndDate}", l.Id.ToString())));

            return selection;
        }
    }
}
