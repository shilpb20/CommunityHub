using Microsoft.AspNetCore.Mvc;
using CommunityHub.Core.Dtos;
using CommunityHub.UI.Services;
using CommunityHub.Core.Constants;
using CommunityHub.UI.Constants;
using CommunityHub.UI.Services.Registration;
using CommunityHub.Core.Helpers;
using CommunityHub.Core;

namespace CommunityHub.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IRegistrationService _service;

        public AccountController(ILogger<AccountController> logger, IRegistrationService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet(UiRoute.Account.Register)]
        public IActionResult Register()
        {
            var registrationData = new RegistrationInfoCreateDto()
            {
                UserInfo = new UserInfoCreateDto(),
                SpouseInfo = null,
                Children = new List<ChildrenCreateDto>()
            };

            return View(registrationData);
        }

        [HttpPost(UiRoute.Account.Register)]
        public async Task<IActionResult> Add([FromForm] RegistrationInfoCreateDto registrationData)
        {
           ErrorResponse? errorResponse = ValidationHelper.ValidateModelState(ModelState, "");
            if (errorResponse != null)
            {
                ViewBag.ErrorMessages = errorResponse.ErrorMessage;
                return View("register", registrationData);
            }

            var result = await _service.SendRegistrationRequestAsync(registrationData);
            if (!result.Success)
            {
                ModelState.AddModelError(result.ErrorCode, result.ErrorMessage);
                return View("register", registrationData);
            }

            TempData["SuccessMessage"] = "Registration request has been sent for admin approval!";
            return RedirectToAction("index");
        }
    }
}
