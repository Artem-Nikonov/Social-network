using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Models;

namespace SocialNetworkServer.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> TryAuthenticateUserAsync(UserAuthorizationModel accountData, HttpContext httpContext);
        void LogOut(HttpContext httpContext);
    }
}
