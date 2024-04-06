using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialNetworkServer.Models;
using SocialNetworkServer.Services;
using SocialNetworkServer.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SocialNetworkServer.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private IRegistrationService registrationService;
        private IAuthenticationService authenticationService;
        public AccountController(IRegistrationService registrationService, IAuthenticationService authenticationService)
        {
            this.registrationService = registrationService;
            this.authenticationService = authenticationService;
        }

        [HttpGet("registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpGet("authorization")]
        public IActionResult Authorization()
        {
            return View();
        }

        [Authorize]
        [HttpGet("settings")]
        public IActionResult AccountSettings()
        {
            return View();
        }

        [HttpGet("logOut")]
        public IActionResult LogOut()
        {
            authenticationService.LogOut(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration(UserRegistrationModel userAccount)
        {
            var IsSuccessful = await registrationService.TryRegisterAccountAsync(userAccount);
            if (IsSuccessful && ModelState.IsValid) return Redirect("/");
            var errorMessage = "Логин занят";
            ModelState.AddModelError("", errorMessage);
            return View(userAccount);
        }

        [HttpPost("authorization")]
        public async Task<IActionResult> Authorization(string? returnUrl,UserAuthorizationModel accountData)
        {
            var IsSuccessful = await authenticationService.TryAuthenticateUserAsync(accountData, HttpContext);
            if (IsSuccessful && ModelState.IsValid) return Redirect(returnUrl ?? "~/Home/Index");
            var errorMessage = "Неверный логин или пароль.";
            ModelState.AddModelError("", errorMessage);
            return View(accountData);
        }

    }
}
