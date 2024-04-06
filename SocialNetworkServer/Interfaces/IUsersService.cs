using SocialNetworkServer.AuxiliaryClasses;
using System.Security.Claims;

namespace SocialNetworkServer.Interfaces
{
    public interface IUsersService
    {
        Task<UserInfo?> GetUserInfo(int id);
        int GetUserId(ClaimsPrincipal user);
        Task<List<UserInfo>> GetUsers(int page);
        static int limit = 5;
    }
}
