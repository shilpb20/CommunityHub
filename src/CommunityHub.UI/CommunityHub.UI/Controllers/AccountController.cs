using Microsoft.AspNetCore.Mvc;
using CommunityHub.Core.Dtos;
using CommunityHub.UI.Services;

namespace CommunityHub.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _url = "api/account";

        private readonly ILogger<AccountController> _logger;
        private readonly IBaseService _service;

        public AccountController(ILogger<AccountController> logger, IBaseService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            var registrationData = new RegistrationDataCreateDto() { UserDetails = new UserDetailsCreateDto() };
            return View(registrationData);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] RegistrationDataCreateDto registrationData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();

                ViewBag.ErrorMessages = errorMessages;
                return View("register", registrationData);
            }

            var result = await _service.AddRequestAsync<RegistrationDataCreateDto, RegistrationRequestDto>(_url, registrationData);
            if (result == null)
            {
                ModelState.AddModelError("", "An error occurred while registering.");
                return View("register", registrationData);
            }

            TempData["SuccessMessage"] = "Registration request has been sent for admin approval!";
            return RedirectToAction("index");
        }
    }
}
