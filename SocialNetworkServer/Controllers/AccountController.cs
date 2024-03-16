using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialNetworkServer.Models;
using SocialNetworkServer.Services;

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
        public IActionResult Registration([FromServices]RegistrationService rs, UserRegistrationModel userAccountData)
        {
            var res = rs.reg(userAccountData.Password);
            return Content(res);
        }
    }
}
