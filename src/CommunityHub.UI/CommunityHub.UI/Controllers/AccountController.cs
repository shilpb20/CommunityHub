using CommunityHub.Core;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using CommunityHub.UI.Services;
using CommunityHub.UI.Services.Registration;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IRegistrationService _registrationService;
        private readonly IAccountService _accountService;

        public AccountController(
            ILogger<AccountController> logger,
            IRegistrationService registrationService,
            IAccountService accountService)
        {
            _logger = logger;
            _registrationService = registrationService;
            _accountService = accountService;
        }


        #region register-account


        [HttpGet(UiRoute.Account.Register)]
        public IActionResult Register()
        {
            var registrationData = new RegistrationInfoCreateDto()
            {
                UserInfo = new UserInfoCreateDto(),
                SpouseInfo = null,
                Children = new List<ChildCreateDto>()
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

            var result = await _registrationService.SendRegistrationRequestAsync(registrationData);
            if (!result.Success)
            {
                ModelState.AddModelError(result.ErrorCode, result.ErrorMessage);
                return View("register", registrationData);
            }

            TempData["SuccessMessage"] = "Registration request has been sent for admin approval!";
            return RedirectToAction("index");
        }

        #endregion

        #region set-password

        [HttpGet(UiRoute.Account.ResetPassword)]
        [HttpGet(UiRoute.Account.SetPassword)]
        public IActionResult SetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Error", "Home");
            }
          
            return View();
        }

        [HttpPost(UiRoute.Account.ResetPassword)]
        [HttpPost(UiRoute.Account.SetPassword)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }

            var result = await _accountService.SetNewPasswordAsync(model);
            if (!result.Success)
            {
                ModelState.AddModelError("Password", result.ErrorMessage);
                return View(model);
            }

            return RedirectToAction(nameof(SetPasswordConfirmation));
        }

        public IActionResult SetPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region login

        [HttpGet(UiRoute.Account.Login)]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost(UiRoute.Account.Login)]
        public async Task<IActionResult> Login([FromForm] Login model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.ErrorMessage = "Email and password are required.";
                return View(model);
            }

            var result = await _accountService.LoginAsync(model);
            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid login credentials";
            return View(model);
        }

        #endregion

        #region forgot-password

        [HttpGet(UiRoute.Account.ForgotPassword)]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost(UiRoute.Account.ForgotPassword)]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Please enter your registered email.");
                return View();
            }

            var apiResponse = await _accountService.SendPasswordResetEmailAsync(email);
            TempData["SuccessMessage"] = "If this email is registered, you will receive a password reset link.";
            return RedirectToAction("ForgotPassword");
        }

        #endregion
    }
}
