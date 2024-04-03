using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Models;
using SocialNetworkServer.Services;
using SocialNetworkServer.AuxiliaryClasses;
using System.Globalization;

namespace SocialNetworkServer.Controllers
{
    [Route("page")]
    public class PageController : Controller
    {
        private UserService userService;
        public PageController(UserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> UserPage(int id)
        {
            var userInfo = await userService.GetUserInfo(id);
            if(userInfo == null) return NotFound();
            var visitorId = userService.GetUserId(HttpContext.User);
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
