using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.DTO.ApprovalRequest;
using OutOfOffice.Services;
using System.Security.Claims;

namespace OutOfOffice.Controllers
{
    public class ApprovalRequestController(ApprovalRequestService approvalRequestService, LeaveRequestService leaveRequestService) : Controller
    {
        private readonly ApprovalRequestService _approvalRequestService = approvalRequestService;
        private readonly LeaveRequestService _leaveRequestService = leaveRequestService;

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);

            var model = await _approvalRequestService.GetAll(role, employeeId);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var result = await _approvalRequestService.GetById(id, role, employeeId);

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return View(result.Value);
        }

        [Authorize(Roles = "0, 1, 2")]
        public async Task<ActionResult> Create()
        {
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            ViewBag.LeaveRequests = await _leaveRequestService.GetNewLeaveRequestsSelectionList(role, employeeId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0, 1, 2")]
        public async Task<IActionResult> Create(ApprovalRequestDTO model)
        {
            var role = int.Parse(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);
            var employeeId = int.Parse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType)!.Value);
            if (ModelState.IsValid)
            {

                var result = await _approvalRequestService.Add(model, employeeId);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Error);
            }
            ViewBag.LeaveRequests = await _leaveRequestService.GetNewLeaveRequestsSelectionList(role, employeeId);
            return View(model);
        }
    }
}
