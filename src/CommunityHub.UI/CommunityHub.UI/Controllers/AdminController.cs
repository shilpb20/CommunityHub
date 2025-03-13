using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Core.Helpers;
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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("request")]
        public async Task<IActionResult> GetRequests([FromQuery] string status = "pending")
        {
            if (!Enum.TryParse<RegistrationStatus>(status, true, out var regStatus)
                || !Enum.IsDefined(typeof(RegistrationStatus), regStatus))
            {
                return BadRequest("Invalid registration status value.");
            }

            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
            {
               { RouteParameter.Request.RegistrationStatus, status }
            };

            string uri = HttpHelper.BuildUri(_service.GetClient().BaseAddress.ToString(), ApiRoutePath.Request, queryParameters);
            var result = await _service.GetRequestAsync<List<RegistrationRequestDto>>(uri);
            return View(result);
        }

        [HttpPost("request/reject")]
        public async Task<IActionResult> RejectRequest([FromForm] int id,  [FromForm]string comment)
        {
            var result = await _service.UpdateRequestAsync<string, RegistrationRequestDto>(ApiRoutePath.RejectRequest, id, comment);
            if(result != null)
            {
                TempData["SuccessMessage"] = "Request rejected successfully!";
            }

            return RedirectToAction("GetRequests", new { status = "pending" });
        }
    }
}
