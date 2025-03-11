using Microsoft.AspNetCore.Mvc;
using CommunityHub.Core.Dtos;
using CommunityHub.UI.Services;

namespace CommunityHub.UI.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly string _url = "api/Register";

        private readonly IBaseService _service;

        public RegistrationController(IBaseService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(null);
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
                return View("Index", registrationData);
            }

            var result = await _service.AddRequestAsync<RegistrationDataCreateDto, RegistrationRequestDto>(_url, registrationData);
            if (result == null)
            {
                ModelState.AddModelError("", "An error occurred while registering.");
                return View("Index", registrationData);
            }

            TempData["SuccessMessage"] = "Registration request has been sent. You will be registered after your request has been approved by admin.";
            return View("Index", new RegistrationDataCreateDto());
        }
    }
}
