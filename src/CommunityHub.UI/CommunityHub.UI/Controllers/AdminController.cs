using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Helpers;
using CommunityHub.UI.Constants;
using CommunityHub.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CommunityHub.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _service;

        public AdminController(ILogger<AdminController> logger, 
            IAdminService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet(UiRoute.Admin.Index)]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet(UiRoute.Admin.RegistrationRequest)]
        public async Task<IActionResult> GetPendingRequests()
        {
            Response.Headers.Add("Cache-Control", "no-store");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "-1");

            var result = await _service.GetPendingRequests();
            return View(result.Data);
        }

        [HttpPost(UiRoute.Admin.RejectRequest)]
        public async Task<IActionResult> RejectRequest([FromForm] int id, [FromForm] string comment)
        {
            var result = await _service.RejectRegistrationRequest(id, comment);
            if (result != null & result.Success)
            {
                TempData["SuccessMessage"] = "Registration request has been successfully rejected!";
            }
            else
            {
                ViewBag.ErrorMessage = "An error occurred while rejecting the request. Please try again. " + result?.ErrorMessage;
            }

            return RedirectToAction(nameof(GetPendingRequests));
        }

        [HttpPost(UiRoute.Admin.ApproveRequest)]
        public async Task<IActionResult> ApproveRequest([FromForm] int id)
        {
            var result = await _service.ApproveRegistrationRequest(id);
            if (result != null && result.Success)
            {
                TempData["SuccessMessage"] = "Registration request has been successfully approved!";
            }
            else
            {
                TempData["ErrorMessage"] = "Registration approval failed!";

                ViewBag.ErrorMessage = result?.ErrorCode + ". "+ result?.ErrorMessage;
            }

            return RedirectToAction(nameof(GetPendingRequests));
        }
    }
}
