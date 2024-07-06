using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Context;
using OutOfOffice.DTO.Employee;
using OutOfOffice.Entities;
using OutOfOffice.Helpers;

namespace OutOfOffice.Services
{
    public class EmployeeService(OutOfOfficeContext context, IMapper mapper)
    {
        private readonly OutOfOfficeContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<EmployeeOutDTO>> GetAll(int employeeId, int role)
        {
            return role switch
            {
                0 => await _context.Employees.Include(e => e.PeoplePartner).Where(e => e.PeoplePartner != null && e.PeoplePartner.Id == employeeId).ProjectTo<EmployeeOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                1 => await _context.Employees.Include(e => e.PeoplePartner).Include(e => e.Projects)
                    .Where(e => e.Id != employeeId && e.Projects.Any(p => p.Employees.Any(pe => pe.Id == employeeId)))
                    .ProjectTo<EmployeeOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                _ => await _context.Employees.Include(c => c.PeoplePartner)
                    .ProjectTo<EmployeeOutDTO>(_mapper.ConfigurationProvider).ToListAsync()
            };
        }


        /// <summary>
        /// Searches for employee by id. The method takes into account the person who is searching, to prevent data from appearing in the wrong hands
        /// </summary>
        /// <param name="id">Id of searched employee</param>
        /// <param name="role">Role of searcher</param>
        /// <param name="employeeId">Id of searcher</param>
        /// <returns>If employee is exist and searcher has permission to have this data, method returns the employee. Otherwise, method returns NULL</returns>
        private async Task<Employee?> GetEmployee(int id, int role, int employeeId)
        {
            return role switch
            {
                0 => await _context.Employees.Include(e => e.PeoplePartner).FirstOrDefaultAsync(e => e.PeoplePartner != null && e.PeoplePartner.Id == employeeId && e.Id == id),
                1 => await _context.Employees.Include(e => e.PeoplePartner).Include(e => e.Projects)
                   .FirstOrDefaultAsync(e => e.Id != employeeId && e.Id == id && e.Projects.Any(p => p.Employees.Any(pe => pe.Id == employeeId))),
                _ => await _context.Employees.Include(e => e.PeoplePartner).FirstOrDefaultAsync(e => e.Id == id)
            };
        }

        public async Task<Result<EmployeeOutDTO>> GetById(int id, int role, int employeeId)
        {
            var employee = await GetEmployee(id, role, employeeId);

            if (employee == null)
            {
                return Result.Failure<EmployeeOutDTO>("Employee with this id was not found");
            }

            return Result.Success(_mapper.Map<EmployeeOutDTO>(employee));
        }

        public async Task<List<SelectListItem>> GetEmployeeSelectionList(int role)
        {
            var selectionList = new List<SelectListItem>();

            await _context.Employees.Where(e => e.Position == role).ForEachAsync(e => selectionList.Add(new SelectListItem(e.Fullname, e.Id.ToString())));

            return selectionList;
        }

        public async Task<Result> Add(EmployeeCreateDTO model, int role, int employeeId)
        {
            if (_context.Employees.Any(e => e.Email == model.Email))
            {
                return Result.Failure("User with this email already exists");
            }
            var salt = PasswordHelper.GenerateSalt();
            var hashedPassword = PasswordHelper.HashPassword(model.Password, salt);
            if (role == 0)
            {
                model.PeoplePartnerId = employeeId;
            }

            var employee = new Employee()
            {
                Email = model.Email,
                Password = hashedPassword,
                Salt = salt,
                Fullname = model.Fullname,
                Subdivision = model.Subdivision,
                Position = model.Position,
                Status = true,
                PeoplePartnerId = model.PeoplePartnerId,
                OutOfOfficeBalance = model.OutOfOfficeBalance,
            };

            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<EmployeeEditDTO>> GetUpdateValues(int id, int role, int employeeId)
        {
            var emp = await GetEmployee(id, role, employeeId);

            if (emp == null)
            {
                return Result.Failure<EmployeeEditDTO>("Employee with this id was not found");
            }

            return Result.Success(_mapper.Map<EmployeeEditDTO>(emp));
        }

        public async Task<Result> Edit(int id, EmployeeEditDTO model, int role, int employeeId)
        {
            var employee = await GetEmployee(id, role, employeeId);

            if (employee == null)
            {
                return Result.Failure("Employee was not found");
            }

            var salt = PasswordHelper.GenerateSalt();
            var hashedPassword = PasswordHelper.HashPassword(model.Password, salt);
            if (role == 0)
            {
                model.PeoplePartnerId = employeeId;
            }

            employee.Password = hashedPassword;
            employee.Salt = salt;
            employee.PeoplePartnerId = model.PeoplePartnerId;
            employee.Fullname = model.Fullname;
            employee.Subdivision = model.Subdivision;
            employee.Position = model.Position;
            employee.OutOfOfficeBalance = model.OutOfOfficeBalance;



            _context.Update(employee);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> Delete(int id, int role, int employeeId)
        {
            var employee = await GetEmployee(id, role, employeeId);

            if (employee == null)
            {
                return Result.Failure("Employee was not found");
            }

            _context.Remove(employee);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> ToggleEmployeeStatus(int id, int role, int employeeId)
        {
            var employee = await GetEmployee(id, role, employeeId);

            if (employee == null)
            {
                return Result.Failure("Employee was not found");
            }

            employee.Status = !employee.Status;
            _context.Update(employee);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<List<EmployeeSelectionListDTO>> GetFreeEmployees(int projectId)
        {
            return await _context.Employees.Include(e => e.Projects)
                .Where(e => (e.Position == 3 || e.Position == 1) && e.Projects.All(p => p.Id != projectId))
                .ProjectTo<EmployeeSelectionListDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
