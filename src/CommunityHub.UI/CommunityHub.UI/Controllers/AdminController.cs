using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Helpers;
using CommunityHub.UI.Constants;
using CommunityHub.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IBaseService _service;

        public AdminController(ILogger<AdminController> logger, IBaseService service)
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
        public async Task<IActionResult> GetRequests([FromQuery] string status = "pending")
        {
            Response.Headers.Add("Cache-Control", "no-store");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "-1");

            if (!Enum.TryParse<RegistrationStatus>(status, true, out var regStatus)
                || !Enum.IsDefined(typeof(RegistrationStatus), regStatus))
            {
                return BadRequest("Invalid registration status value.");
            }

            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
            {
               { RouteParameter.Registration.Status, status }
            };

            string uri = HttpHelper.BuildUri(_service.GetClient().BaseAddress.ToString(), ApiRoute.Registration.Request, queryParameters);
            var result = await _service.GetRequestAsync<List<RegistrationRequestDto>>(uri);
            return View(result);
        }

        [HttpPost(UiRoute.Admin.RejectRequest)]
        public async Task<IActionResult> RejectRequest([FromForm] int id, [FromForm] string comment)
        {
            var result = await _service.UpdateRequestAsync<string, RegistrationRequestDto>(
                ApiRoute.Admin.RejectRequestById, id, comment);
            if (result != null)
            {
                TempData["SuccessMessage"] = "Registration request has been successfully rejected!";
            }

            return RedirectToAction("GetRequests", new { status = "pending" });
        }

        [HttpPost(UiRoute.Admin.ApproveRequest)]
        public async Task<IActionResult> ApproveRequest([FromForm] int id)
        {
            //TODO: Check duplicate user

            var result = await _service.AddRequestAsync<string, UserInfoDto>(ApiRoute.Admin.ApproveRequestById, id, null);
            if (result != null)
            {
                TempData["SuccessMessage"] = "Registration request has been successfully approved. User details have been added to the system.";
            }

            return RedirectToAction("GetRequests", new { status = "pending" });
        }
    }
}
