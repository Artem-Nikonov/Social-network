using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Enums;

namespace SocialNetworkServer.Controllers
{
    public class SubscriptionsController : Controller
    {
        private IUserSubscriptionService userSubscriptionService;
        private IGroupSubscriptionService groupSubscriptionService;
        private IUsersService usersService;

        public SubscriptionsController(IUsersService usersService, IUserSubscriptionService userSubscriptionService, IGroupSubscriptionService groupSubscriptionService)
        {
            this.usersService = usersService;
            this.userSubscriptionService = userSubscriptionService;
            this.groupSubscriptionService = groupSubscriptionService;
        }

        [Authorize]
        [HttpPost("subscribe/{pageId:int}")]
        public async Task<IActionResult> SubscribeToUser(int pageId, [FromQuery] PageTypes pageType)
        {
            var subscriberId = usersService.GetUserId(HttpContext.User);
            bool isSuccess = false;
            switch (pageType)
            {
                case PageTypes.userPage:
                    isSuccess = await userSubscriptionService.SubscribeToUser(subscriberId, pageId);
                    break;
                case PageTypes.group:
                    isSuccess = await groupSubscriptionService.SubscribeToGroup(subscriberId, pageId);
                    break;
            }
            if (isSuccess) return Ok();
            return Conflict("Вы уже подписаны на эту страницу");
        }

        [Authorize]
        [HttpDelete("unsubscribe/{pageId:int}")]
        public async Task<IActionResult> UnsubscribeFromUser(int pageId, [FromQuery] PageTypes pageType)
        {
            var subscriberId = usersService.GetUserId(HttpContext.User);
            bool isSuccess = false;
            switch (pageType)
            {
                case PageTypes.userPage:
                    isSuccess = await userSubscriptionService.UnsubscribeFromUser(subscriberId, pageId);
                    break;
                case PageTypes.group:
                    isSuccess = await groupSubscriptionService.UnsubscribeFromGroup(subscriberId, pageId);
                    break;
            }
            if (isSuccess) return Ok();
            return Conflict("Вы не подписаны на эту страницу");
        }
    }
}
