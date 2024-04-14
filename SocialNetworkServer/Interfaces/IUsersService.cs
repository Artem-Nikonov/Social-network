using SocialNetworkServer.Models;
using System.Security.Claims;

namespace SocialNetworkServer.Interfaces
{
    public interface IUsersService
    {
        Task<UserInfoModel?> GetUserInfo(int id);
        int GetUserId(ClaimsPrincipal user);
        Task<List<UserInfoModel>> GetUsers(int page);
    }
}
