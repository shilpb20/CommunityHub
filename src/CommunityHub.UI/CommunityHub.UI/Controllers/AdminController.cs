using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.UI.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace CommunityHub.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _url = "api/admin";

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

        //[HttpPost("ApproveRequest")]
        //public IActionResult ApproveRequest(int id)
        //{
        //    return View();
        //}

        //[HttpPost("RejectRequest")]
        //public IActionResult RejectRequest(int id, string review)
        //{
        //    return View();
        //}

        [HttpGet("GetRequests")]
        public async Task<IActionResult> GetRequests([FromQuery]string registrationStatus = "pending")
        {
            if (!Enum.TryParse<RegistrationStatus>(registrationStatus, true, out var status)
                || !Enum.IsDefined(typeof(RegistrationStatus), status))
            {
                return BadRequest("Invalid registration status value.");
            }

            var uriBuilder = new UriBuilder(_service.GetClient().BaseAddress)
            {
                Path = _url.TrimStart('/'),
                Query = $"registrationStatus={registrationStatus}"
            };

            var requests = await _service.GetRequestAsync<List<RegistrationRequestDto>>(uriBuilder.ToString());
            return View(requests);
        }
    }
}
