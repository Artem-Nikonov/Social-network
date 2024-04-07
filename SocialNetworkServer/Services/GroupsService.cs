using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;

namespace SocialNetworkServer.Services
{
    public class GroupsService: IGroupsService
    {
        private SocialNetworkDBContext dbContext;
        private IMemoryCache cache;
        private IUsersService userService;
        private ILogger<GroupsService> logger;
        public static int limit { get; private set; } = 5;

        public GroupsService(SocialNetworkDBContext dbContext, IMemoryCache memoryCache, IUsersService userService, ILogger<GroupsService> logger)
        {
            this.dbContext = dbContext;
            this.cache = memoryCache;
            this.userService = userService;
            this.logger = logger;
        }

        public async Task<List<GroupInfoModel>> GetGroups(int page)
        {
            if (page <= 0) page = 1;
            var groups = await dbContext.Groups.OrderByDescending(g => g.GroupId)
                .Skip((page - 1) * PaginationConstants.GroupsPerPage)
                .Take(PaginationConstants.GroupsPerPage).Select(g => new GroupInfoModel
                {
                    GroupId = g.GroupId,
                    GroupName = g.GroupName,
                }).AsNoTracking().ToListAsync();
            return groups;
        }

        public async Task<GroupInfoModel?> GetGroupInfo(int id)
        {
            var groupId = $"g{id}";//добавляем букву g чтобы отличать id пользователя от id группы
            cache.TryGetValue(groupId, out GroupInfoModel? groupInfo);
            if (groupInfo == null)
            {
                var group = await dbContext.Groups.FindAsync(id);
                if (group != null)
                {
                    groupInfo = group;
                    cache.Set(groupId, groupInfo, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                    Console.WriteLine($"{groupInfo.GroupId} ГРУППА ПОЛУЧЕНА ИЗ БД!!!!!!!!!!!");
                }
            }
            else
                Console.WriteLine($"{groupInfo.GroupId} ГРУППА ПОЛУЧЕНА ИЗ КЭША!!!!!!!!!!!");
            return groupInfo;
        }

        public async Task<GroupInfoModel?> CreateGroup(GroupInfoModel groupInfo, ClaimsPrincipal user)
        {
            int creatorId;
            try
            {
                creatorId = userService.GetUserId(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
            var group = new Group
            {
                GroupName = groupInfo.GroupName,
                Description = groupInfo.Description,
                CreatorId = creatorId,
                PostPermissions = groupInfo.PostPermissions,
            };
            await dbContext.Groups.AddAsync(group);
            await dbContext.SaveChangesAsync();
            return groupInfo;
        }
    }
}
