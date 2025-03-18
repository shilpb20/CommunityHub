using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using CommunityHub.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.UI.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IBaseService _service;

        public AdminController(ILogger<AdminController> logger, IBaseService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("request")]
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
               { RouteParameter.Request.RegistrationStatus, status }
            };

            string uri = HttpHelper.BuildUri(_service.GetClient().BaseAddress.ToString(), ApiRouteSegment.AdminRequest, queryParameters);
            var result = await _service.GetRequestAsync<List<RegistrationRequestDto>>(uri);
            return View(result);
        }

        [HttpPost("request/reject")]
        public async Task<IActionResult> RejectRequest([FromForm] int id, [FromForm] string comment)
        {
            var result = await _service.UpdateRequestAsync<string, RegistrationRequestDto>(ApiRouteSegment.RejectRequest, id, comment);
            if (result != null)
            {
                TempData["SuccessMessage"] = "Registration request has been successfully rejected!";
            }

            return RedirectToAction("GetRequests", new { status = "pending" });
        }

        [HttpPost("request/approve")]
        public async Task<IActionResult> ApproveRequest([FromForm] int id)
        {
            var result = await _service.AddRequestAsync<string, UserInfo>($"{ApiRouteSegment.ApproveRequest}/{id}", null);
            if (result != null)
            {
                TempData["SuccessMessage"] = "Registration request has been successfully approved. User details have been added to the system.";
            }

            return RedirectToAction("GetRequests", new { status = "pending" });
        }
    }
}
