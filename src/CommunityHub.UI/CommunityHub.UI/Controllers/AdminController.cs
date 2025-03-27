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
            if (result != null)
            {
                TempData["SuccessMessage"] = "Registration request has been successfully rejected!";
            }

            return RedirectToAction(nameof(GetPendingRequests));
        }

        [HttpPost(UiRoute.Admin.ApproveRequest)]
        public async Task<IActionResult> ApproveRequest([FromForm] int id)
        {
            //TODO: Check duplicate user

            //var result = await _service.<string, UserInfoDto>(ApiRoute.Admin.ApproveRequestById, id, null);
            //if (result != null)
            //{
            //    TempData["SuccessMessage"] = "Registration request has been successfully approved. User details have been added to the system.";
            //}

            return RedirectToAction("GetRequests", new { status = "pending" });
        }
    }
}
