using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Enums;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.CustomTagHelpers
{
    public class SubscriptionButtonTagHelper: TagHelper
    {
        private SocialNetworkDBContext dbContext;
        public SubscriptionButtonTagHelper(SocialNetworkDBContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public int UserId { get; set; }
        public int PageId { get; set;}
        public PageTypes PageType { get; set; } = PageTypes.userPage;
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "button";
            output.Attributes.SetAttribute("class", "custom_button");
            var userIsSubscribed = false;

            if (PageType == PageTypes.userPage && UserId==PageId)
            {
                output.SuppressOutput();
                return;
            }
            else if(PageType==PageTypes.userPage)
            {
                userIsSubscribed = await TryFindSubscriptionToUser(UserId, PageId);
            }
            else
            {
                userIsSubscribed = await TryFindSubscriptionToGroup(UserId, PageId);
            }
            output.Content.SetContent(userIsSubscribed ? "Отписаться" : "Подписаться");
        }

        private async Task<bool> TryFindSubscriptionToUser(int followerId, int followeeId)
        {
            var subscription = await dbContext.UserSubscriptions
               .FirstOrDefaultAsync(s => s.SubscriberId == followerId &&
               s.SubscribedToUserId == followeeId);
            return subscription != null;
        }

        private async Task<bool> TryFindSubscriptionToGroup(int userId, int groupId)
        {
            var subscription = await dbContext.GroupSubscriptions
               .FirstOrDefaultAsync(s => s.SubscriberId == userId &&
               s.SubscribedToGroupId == groupId);
            return subscription != null;
        }
    }
}
