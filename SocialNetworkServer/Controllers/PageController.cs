using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Models;
using SocialNetworkServer.Services;
using SocialNetworkServer.AuxiliaryClasses;
using System.Globalization;

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
            var userInfo = await userService.GetUserInfo(id);
            if(userInfo == null) return NotFound();
            int.TryParse(userService.GetUserId(HttpContext.User), out int visitorId);
            var visitorIsOwner = visitorId == userInfo.UserId;
            var userPageModel = new UserPageModel()
            {
                userInfo = userInfo,
                metaData = new PageMetaData()
                {
                    PageId = id,
                    VisitorId = visitorId,
                    VisitorIsOwner = visitorIsOwner
                }
            };
            return View(userPageModel);
        }

    }
}
