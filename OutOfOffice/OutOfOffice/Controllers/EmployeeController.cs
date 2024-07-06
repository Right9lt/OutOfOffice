using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOffice.DTO.Employee;
using OutOfOffice.Services;
using System.Security.Claims;

namespace OutOfOffice.Controllers
{
    public class EmployeeController(EmployeeService employeeService) : Controller
    {
        private readonly EmployeeService _employeeService = employeeService;

        [Authorize(Roles = "0, 1, 2")]
        public async Task<IActionResult> Index()
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var model = await _employeeService.GetAll(employeeId, role);
            return View(model);
        }

        [Authorize(Roles = "0, 1, 2")]
        public async Task<IActionResult> Details(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _employeeService.GetById(id, role, employeeId);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return View(result.Value);
        }

        [Authorize(Roles = "0, 2")]
        public async Task<IActionResult> Create()
        {

            ViewBag.PeoplePartners = await _employeeService.GetEmployeeSelectionList(0);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0, 2")]
        public async Task<IActionResult> Create(EmployeeCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var id = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
                var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
                var result = await _employeeService.Add(model, role, id);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Error);
            }

            ViewBag.PeoplePartners = await _employeeService.GetEmployeeSelectionList(0);
            return View(model);
        }

        [Authorize(Roles = "0, 2")]
        public async Task<IActionResult> Edit(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _employeeService.GetUpdateValues(id, role, employeeId);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            ViewBag.PeoplePartners = await _employeeService.GetEmployeeSelectionList(0);
            return View(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0, 2")]
        public async Task<IActionResult> Edit(int id, EmployeeEditDTO model)
        {
            if (ModelState.IsValid)
            {
                var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
                var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
                var result = await _employeeService.Edit(id, model, role, employeeId);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Error);
            }

            ViewBag.PeoplePartners = await _employeeService.GetEmployeeSelectionList(0);
            return View(model);
        }

        [Authorize(Roles = "0, 2")]
        public async Task<IActionResult> Delete(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _employeeService.Delete(id, role, employeeId);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return NotFound(result.Error);
        }

        [Authorize(Roles = "0, 2")]
        public async Task<IActionResult> ToggleEmployeeStatus(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _employeeService.ToggleEmployeeStatus(id, role, employeeId);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return NotFound(result.Error);
        }

    }
}
