using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.Interfaces;
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

        public Task<List<UserInfo>> GetGroupSubscribers(int groupId)
        {
            throw new NotImplementedException();
        }

        
    }
}
