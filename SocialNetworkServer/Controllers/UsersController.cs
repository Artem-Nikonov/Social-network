using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Services;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private UserService userService;
        public UsersController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers(int page)
        {
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var users = await userService.GetUsers(page);
            var usersData = new
            {
                Meta = new
                {
                    PageId = page,
                    IsLastPage = users.Count < UserService.limit
                },
                Users=users
            };
            return Json(usersData);
        }
    }
}
