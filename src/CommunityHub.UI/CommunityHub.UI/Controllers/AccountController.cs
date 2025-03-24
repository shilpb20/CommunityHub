using Microsoft.AspNetCore.Mvc;
using CommunityHub.Core.Dtos;
using CommunityHub.UI.Services;
using CommunityHub.Core.Constants;
using CommunityHub.UI.Constants;

namespace CommunityHub.UI.Controllers
{
    public class AccountController : Controller
    {
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
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();

                ViewBag.ErrorMessages = errorMessages;
                return View("register", registrationData);
            }

            //TODO: Check duplicate details - user/partner
            //var duplicateUser = await FindUserAsync(
            //    registrationData.UserInfo.Email, 
            //    registrationData.UserInfo.ContactNumber);

            //if (duplicateUser != null)
            //{
            //    //Add error
            //    return View("register", registrationData);
            //}

            var result = await _service.AddRequestAsync<RegistrationInfoCreateDto, RegistrationRequestDto>(
                ApiRoute.Registration.CreateRequest, registrationData);
            if (result == null)
            {
                ModelState.AddModelError("", "An error occurred while registering.");
                return View("register", registrationData);
            }

            TempData["SuccessMessage"] = "Registration request has been sent for admin approval!";
            return RedirectToAction("index");
        }

        private async Task<UserInfoDto> FindUserAsync(string email, string contactNumber)
        {
            string uri = ApiRoute.Users.Find;
            uri += $"?email={email}&contact={contactNumber}";

            var user = await _service.GetRequestAsync<UserInfoDto>(uri);
            return user ?? null;
        }
    }
}
