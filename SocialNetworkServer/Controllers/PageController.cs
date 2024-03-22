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
            ViewData["IsOvner"] = userService.GetUserId(HttpContext.User) == userModel.UserId.ToString();
            return View(userModel);
        }

        //[Authorize]
        //[HttpGet]
        //public IActionResult MyPage()
        //{
        //    var userModel = await userService.GetUserInfo(id);
        //    ViewData["IsOvner"] = userService.GetUserId(HttpContext.User) == userModel.UserId.ToString();
        //    return View(userModel);
        //}
    }
}
