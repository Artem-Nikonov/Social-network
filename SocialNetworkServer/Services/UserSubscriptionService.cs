using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Services
{
    public class UserSubscriptionService : IUserSubscriptionService, ISubscribeChecker
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

        public async Task<List<UserInfo>> GetUserFollowers(int userId, int page)
        {
            if (page <= 0) page = 1;
            var users = await dbContext.UserSubscriptions.Include(us=>us.Subscriber)
                .Where(us=>us.SubscribedToUserId == userId)
                .Skip((page - 1) * PaginationConstants.UsersPerPage)
                .Take(PaginationConstants.UsersPerPage).Select(us => new UserInfo
                {
                    UserId = us.SubscriberId,
                    UserName = us.Subscriber.UserName,
                    UserSurname = us.Subscriber.UserSurname
                }).AsNoTracking().ToListAsync();
            return users;
        }

        public Task<List<UserInfo>> GetUserFollowing(int userId, int page)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserIsSubscribeToGroup(int userId, int GroupId)
        {
            var subscription = await dbContext.GroupSubscriptions.AsNoTracking()
                .FirstOrDefaultAsync(s =>s.SubscriberId == userId &&
                s.SubscribedToGroupId==GroupId);
            return subscription != null;
        }

        private async Task<UserSubscription?> TryFindSubscription(int followerId, int followeeId)
        {
            var subscription = await dbContext.UserSubscriptions
               .FirstOrDefaultAsync(s => s.SubscriberId == followerId &&
               s.SubscribedToUserId == followeeId);
            return subscription;
        }
    }
}
