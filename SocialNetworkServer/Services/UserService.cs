using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.SocNetworkDBContext;
using System.Security.Claims;

namespace SocialNetworkServer.Services
{
    public class UserService
    {
        private SocialNetworkDBContext dbContext;
        private IMemoryCache cache;

        public UserService(SocialNetworkDBContext dbContext, IMemoryCache memoryCache)
        {
            this.dbContext = dbContext;
            this.cache = memoryCache;
        }

        public async Task<UserInfo?> GetUserInfo(int id)
        {
            cache.TryGetValue(id, out UserInfo? userInfo);
            if (userInfo == null)
            {
                var user = await dbContext.Users.FindAsync(id);
                if (user != null)
                {
                    userInfo = user;
                    cache.Set(id, userInfo, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                    Console.WriteLine($"{userInfo.UserId} ЮЗЕР ПОЛУЧЕН ИЗ БД!!!!!!!!!!!");
                }
            }
            else
                Console.WriteLine($"{userInfo.UserId} ЮЗЕР ПОЛУЧЕН ИЗ КЭША!!!!!!!!!!!");
            return userInfo;
        }

        public int GetUserId(ClaimsPrincipal user)
        {
            var strUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (strUserId != null && int.TryParse(strUserId, out int userId)) return userId;
            throw new InvalidOperationException("Ошибка при получении id пользователя.");
        }
    }
}
