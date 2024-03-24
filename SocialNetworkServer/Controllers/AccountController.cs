using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialNetworkServer.Models;
using SocialNetworkServer.Services;

namespace SocialNetworkServer.Controllers
{
    public class AccountController : Controller
    {
        private RegistrationService registrationService;
        private AuthorizationService authorizationService;
        public AccountController(RegistrationService registrationService, AuthorizationService authorizationService)
        {
            this.registrationService = registrationService;
            this.authorizationService = authorizationService;
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
        [HttpPost]
        public async Task<IActionResult> Registration(UserRegistrationModel userAccount)
        {
            var IsSuccessful = await registrationService.TryRegisterAccountAsync(userAccount);
            if (IsSuccessful && ModelState.IsValid) return Redirect("/");
            var errorMessage = registrationService.ErrorMessage ?? "Ошибка регистрации!";
            ModelState.AddModelError("", errorMessage);
            return View(userAccount);
        }

        [HttpPost]
        public async Task<IActionResult> Authorization(string? returnUrl,UserAuthorizationModel accountData)
        {
            var IsSuccessful = await authorizationService.TryAuthorizeUserAsync(accountData, HttpContext);
            if (IsSuccessful && ModelState.IsValid) return Redirect(returnUrl ?? "~/Home/Index");
            var errorMessage = authorizationService.ErrorMessage ?? "Ошибка авторизации!";
            ModelState.AddModelError("", errorMessage);
            return View(accountData);
        }

        [HttpPut]
        public IActionResult ChangeUserInfo([FromBody]UserInfoModel userInfo)
        {
            Console.WriteLine(userInfo.userName);
            return Json(userInfo);
        }
    }
}
