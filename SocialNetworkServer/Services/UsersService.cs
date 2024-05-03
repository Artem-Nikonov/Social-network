using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.RegularExpressions;

namespace SocialNetworkServer.Services
{
    public class UsersService: IUsersService
    {
        private SocialNetworkDBContext dbContext;
        private IMemoryCache cache;

        public UsersService(SocialNetworkDBContext dbContext, IMemoryCache memoryCache)
        {
            this.dbContext = dbContext;
            this.cache = memoryCache;
        }

        public async Task<UserInfoModel?> GetUserInfo(int id)
        {
            cache.TryGetValue(id, out UserInfoModel? userInfo);
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

        public async Task<List<UserInfoModel>>GetUsers(int page, string? filter=null)
        {
            if (page <= 0) page = 1;
            IQueryable<User> query =  dbContext.Users.OrderByDescending(u => u.UserId);

            if (!string.IsNullOrEmpty(filter))
                query = query.Where(user =>
                (user.UserName.ToLower() + " " + user.UserSurname.ToLower()).Contains(filter));

            var users = await query.Skip((page - 1) * PaginationConstants.UsersPerPage)
           .Take(PaginationConstants.UsersPerPage)
           .Select(user => (UserInfoModel)user)
           .AsNoTracking().ToListAsync();
            return users;
        }

        public int GetUserId(ClaimsPrincipal user)
        {
            var strUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (strUserId != null && int.TryParse(strUserId, out int userId)) return userId;
            throw new InvalidOperationException("Ошибка при получении id пользователя.");
        }
    }
}
