using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialNetworkServer.Models;
using SocialNetworkServer.Services;
using SocialNetworkServer.Interfaces;

namespace SocialNetworkServer.Controllers
{
    public class AccountController : Controller
    {
        private IRegistrationService registrationService;
        private IAuthenticationService authenticationService;
        public AccountController(IRegistrationService registrationService, IAuthenticationService authenticationService)
        {
            this.registrationService = registrationService;
            this.authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Authorization()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccountSettings()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            authenticationService.LogOut(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserRegistrationModel userAccount)
        {
            var IsSuccessful = await registrationService.TryRegisterAccountAsync(userAccount);
            if (IsSuccessful && ModelState.IsValid) return Redirect("/");
            var errorMessage = "Логин занят";
            ModelState.AddModelError("", errorMessage);
            return View(userAccount);
        }

        [HttpPost]
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
