using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Services;

namespace SocialNetworkServer.Controllers
{
    [Route("Page")]
    public class PageController : Controller
    {
        private UserService userService;
        public PageController(UserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("{id:int=1}")]
        public async Task<IActionResult> UserPage(int id)
        {
            var userModel = await userService.GetUserInfo(id);
            if(userModel == null) return NotFound();
            ViewBag.IsOwner = userService.GetUserId(HttpContext.User) == userModel.UserId.ToString();
            ViewBag.PageId = id;
            return View(userModel);
        }

    }
}
