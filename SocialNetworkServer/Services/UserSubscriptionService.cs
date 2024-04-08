using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Services
{
    public class UserSubscriptionService : IUserSubscriptionService
    {
        private SocialNetworkDBContext dbContext;

        public UserSubscriptionService(SocialNetworkDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> SubscribeToUser(int followerId, int followeeId)
        {
            if(followerId == followeeId) return false;
            var subscription = await TryFindSubscription(followerId, followeeId);
            if (subscription != null) return false;
            subscription = new UserSubscription
            {
                SubscriberId = followerId,
                SubscribedToUserId = followeeId,
            };
            await dbContext.UserSubscriptions.AddAsync(subscription);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnsubscribeFromUser(int followerId, int followeeId)
        {
            if (followerId == followeeId) return false;
            var subscription = await TryFindSubscription(followerId, followeeId);
            if (subscription == null) return false;
            dbContext.UserSubscriptions.Remove(subscription);
            await dbContext.SaveChangesAsync();
            return true;
        }

        private async Task<UserSubscription?> TryFindSubscription(int followerId, int followeeId)
        {
            var subscription = await dbContext.UserSubscriptions
               .FirstOrDefaultAsync(s => s.SubscriberId == followerId &&
               s.SubscribedToUserId == followeeId);
            return subscription;
        }

        public Task<List<UserInfo>> GetUserFollowers(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInfo>> GetUserFollowing(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
