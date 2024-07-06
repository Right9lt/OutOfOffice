using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Context;
using OutOfOffice.DTO.Project;
using OutOfOffice.Entities;

namespace OutOfOffice.Services
{
    public class ProjectService(OutOfOfficeContext context, IMapper mapper)
    {
        private readonly OutOfOfficeContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ProjectOutDTO>> GetAll(int employeeId, int role)
        {
            return role switch
            {
                0 => await _context.Projects.Include(p => p.Employees)
                .Where(p => p.Employees.Any(e => e.PeoplePartnerId == employeeId)).ProjectTo<ProjectOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                2 => await _context.Projects.Include(p => p.Employees).ProjectTo<ProjectOutDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                _ => await _context.Projects.Include(e => e.Employees).Where(p => p.Employees.Any(e => e.Id == employeeId)).ProjectTo<ProjectOutDTO>(_mapper.ConfigurationProvider).ToListAsync()
            };
        }

        /// <summary>
        /// Searches for project by id. The method takes into account the person who is searching, to prevent data from appearing in the wrong hands
        /// </summary>
        /// <param name="id">Id of searched project</param>
        /// <param name="role">Role of searcher</param>
        /// <param name="employeeId">Id of searcher</param>
        /// <returns>If project is exist and searcher has permission to have this data, method returns the employee. Otherwise, method returns NULL</returns>
        private async Task<Project?> GetProject(int id, int role, int employeeId)
        {
            return role switch
            {
                0 => await _context.Projects.Include(p => p.Employees).ThenInclude(e => e.PeoplePartner)
                .FirstOrDefaultAsync(p => p.Id == id && p.Employees.Any(e => e.PeoplePartnerId == employeeId)),
                2 => await _context.Projects.Include(p => p.Employees).ThenInclude(e => e.PeoplePartner).FirstOrDefaultAsync(p=>p.Id == id),
                _ => await _context.Projects.Include(e => e.Employees).ThenInclude(e => e.PeoplePartner).FirstOrDefaultAsync(p => p.Id == id && p.Employees.Any(e => e.Id == employeeId))
            };
        }

        public async Task<Result<ProjectOutDTO>> GetById(int id, int role, int employeeId)
        {
            var project = await GetProject(id, role,employeeId);

            if (project == null)
            {
                return Result.Failure<ProjectOutDTO>("Project was not found");
            }

            return Result.Success(_mapper.Map<ProjectOutDTO>(project));
        }

        public async Task<Result> Add(ProjectCreateDTO model, int role, int employeeId)
        {
            if (role == 1)
            {
                model.ProjectManager = employeeId;
            }

            var pm = _context.Employees.FirstOrDefault(e => e.Id == model.ProjectManager);

            if (pm == null)
            {
                return Result.Failure("Project manager was not found");
            }

            var project = new Project()
            {
                ProjectType = model.ProjectType,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Comment = model.Comment,
            };
            project.Employees.Add(pm);

            await _context.AddAsync(project);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<ProjectEditDTO>> GetUpdateValues(int id, int role, int employeeId)
        {
            var project = await GetProject(id, role, employeeId);

            if (project == null)
            {
                return Result.Failure<ProjectEditDTO>("Project was not found");
            }

            return Result.Success(_mapper.Map<ProjectEditDTO>(project));
        }


        public async Task<Result> Edit(int id, ProjectEditDTO model, int role, int employeeId)
        {
            var project = await GetProject(id, role, employeeId);

            if (project == null)
            {
                return Result.Failure<ProjectCreateDTO>("Project was not found");
            }

            project.ProjectType = model.ProjectType;
            project.StartDate = model.StartDate;
            project.EndDate = model.EndDate;
            project.Comment = model.Comment;

            _context.Update(project);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> Delete(int id, int role, int employeeId)
        {
            var project = await GetProject(id, role, employeeId);

            if (project == null)
            {
                return Result.Failure<ProjectCreateDTO>("Project was not found");
            }

            _context.Remove(project);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> ToggleProjectStatus(int id, int role, int employeeId)
        {
            var project = await GetProject(id, role, employeeId);

            if (project == null)
            {
                return Result.Failure<ProjectCreateDTO>("Project was not found");
            }

            project.Status = !project.Status;
            _context.Update(project);

            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> RemoveEmployee(int id, int employeeIdToRemove, int role, int employeeId)
        {
            var project = await GetProject(id, role, employeeId);
            if (project == null)
            {
                return Result.Failure("Project was not found");
            }

            var employee = _context.Employees.FirstOrDefault(e => e.Id == employeeIdToRemove);
            if (employee == null)
            {
                return Result.Failure("Employee was not found");
            }

            project.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> AssignEmployee(int id, int employeeIdToAdd, int role, int employeeId)
        {
            var project = await GetProject(id, role, employeeId); ;
            if (project == null)
            {
                return Result.Failure("Project was not found");
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeIdToAdd);
            if (employee == null)
            {
                return Result.Failure("Employee was not found");
            }

            project.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
