using SocialNetworkServer.Models;
using System.Security.Claims;

namespace SocialNetworkServer.Interfaces
{
    public interface IGroupsService
    {
        Task<GroupInfoModel?> CreateGroup(GroupInfoModel groupInfo, ClaimsPrincipal user);
        Task<GroupInfoModel?> GetGroupInfo(int id);
        Task<List<GroupInfoModel>> GetGroups(int page);
        static int limit = 2;
    }
}
