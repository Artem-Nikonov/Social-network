using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialNetworkServer.Models;

namespace SocialNetworkServer.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public string Registration(UserRegistrationModel userAccountData)
        {
            if (ModelState.IsValid)
                return "ок";
            return "не ок";

        }
    }
}
