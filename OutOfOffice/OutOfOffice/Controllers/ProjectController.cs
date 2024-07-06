using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOffice.DTO.Project;
using OutOfOffice.Services;
using System.Security.Claims;

namespace OutOfOffice.Controllers
{
    public class ProjectController(ProjectService projectService, EmployeeService employeeService) : Controller
    {
        private readonly ProjectService _projectService = projectService;
        private readonly EmployeeService _employeeService = employeeService;

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);

            var model = await _projectService.GetAll(employeeId, role);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _projectService.GetById(id, role, employeeId);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            ViewBag.Employees = await _employeeService.GetFreeEmployees(id);

            return View(result.Value);
        }

        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Create()
        {
            ViewBag.ProjectManagers = await _employeeService.GetEmployeeSelectionList(1);

            return View();
        }

        [Authorize(Roles = "1, 2")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var id = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
                var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
                var result = await _projectService.Add(model, role, id);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Error);
            }

            ViewBag.ProjectManagers = await _employeeService.GetEmployeeSelectionList(1);
            return View(model);
        }

        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Edit(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _projectService.GetUpdateValues(id, role, employeeId);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return View(result.Value);
        }


        [Authorize(Roles = "1, 2")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectEditDTO model)
        {
            if (ModelState.IsValid)
            {
                var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
                var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
                var result = await _projectService.Edit(id, model,role,employeeId);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Error);
            }

            return View(model);
        }

        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Delete(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _projectService.Delete(id, role, employeeId);
            if (!result.IsSuccess)
            {
                NotFound(result.Error);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> ToggleProjectStatus(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _projectService.ToggleProjectStatus(id,role,employeeId);
            if (!result.IsSuccess)
            {
                NotFound(result.Error);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> RemoveEmployee(int id, int employeeIdToRemove)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _projectService.RemoveEmployee(id, employeeIdToRemove, role, employeeId);
            if (!result.IsSuccess)
            {
                NotFound(result.Error);
            }
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> AssignEmployee(int id, int employeeIdToAssign)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _projectService.AssignEmployee(id, employeeIdToAssign, role, employeeId);
            if (!result.IsSuccess)
            {
                NotFound(result.Error);
            }
            return RedirectToAction("Details", new { id = id });
        }
    }
}
