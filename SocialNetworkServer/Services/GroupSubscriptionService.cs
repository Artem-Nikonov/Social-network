using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Services
{
    public class GroupSubscriptionService : IGroupSubscriptionService
    {
        private SocialNetworkDBContext dbContext;

        public GroupSubscriptionService(SocialNetworkDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> SubscribeToGroup(int userId, int groupId)
        {
            var subscription = await TryFindSubscription(userId, groupId);
            if (subscription != null) return false;
            subscription = new GroupSubscription
            {
                SubscriberId = userId,
                SubscribedToGroupId= groupId,
            };
            await dbContext.GroupSubscriptions.AddAsync(subscription);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnsubscribeFromGroup(int userId, int groupId)
        {
            var subscription = await TryFindSubscription(userId, groupId);
            if (subscription == null) return false;
            dbContext.GroupSubscriptions.Remove(subscription);
            await dbContext.SaveChangesAsync();
            return true;
        }

        private async Task<GroupSubscription?> TryFindSubscription(int followerId, int groupId)
        {
            var subscription = await dbContext.GroupSubscriptions
               .FirstOrDefaultAsync(s => s.SubscriberId == followerId &&
               s.SubscribedToGroupId == groupId);
            return subscription;
        }

        public async Task<List<UserInfoModel>> GetGroupSubscribers(int groupId, int page)
        {
            if (page <= 0) page = 1;
            var users = await dbContext.GroupSubscriptions.Include(gs => gs.Subscriber)
                .Where(gs => gs.SubscribedToGroupId == groupId)
                .Skip((page - 1) * PaginationConstants.UsersPerPage)
                .Take(PaginationConstants.UsersPerPage).Select(gs => new UserInfoModel
                {
                    UserId = gs.SubscriberId,
                    UserName = gs.Subscriber.UserName,
                    UserSurname = gs.Subscriber.UserSurname
                }).AsNoTracking().ToListAsync();
            return users;
        }

        
    }
}
