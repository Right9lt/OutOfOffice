using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.DTO.LeaveRequest;
using OutOfOffice.Services;
using System.Security.Claims;

namespace OutOfOffice.Controllers
{
    public class LeaveRequestController(LeaveRequestService leaveRequestService) : Controller
    {
        private readonly LeaveRequestService _leaveRequestService = leaveRequestService;

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var model = await _leaveRequestService.GetAll(role, employeeId);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _leaveRequestService.GetById(id, role, employeeId);

            if (!result.IsSuccess)
            {
                NotFound(result.Error);
            }

            return View(result.Value);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
                var result = await _leaveRequestService.Add(model, employeeId);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Error);
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Cancel(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _leaveRequestService.Cancel(id, role, employeeId);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return NotFound(result.Error);
        }
    }
}
