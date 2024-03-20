using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace SocialNetworkServer.Services
{
    public class AuthorizationService
    {
        private SocialNetworkDBContext dbContext;
        private IPasswordHasher passwordHasher;
        public string? ErrorMessage { get; private set; }
        public AuthorizationService(SocialNetworkDBContext dBContext, IPasswordHasher passwordHasher)
        {
            this.dbContext = dBContext;
            this.passwordHasher = passwordHasher;
        }

        public async Task<bool> TryAuthorizeUserAsync(UserAuthorizationModel accountData, HttpContext httpContext)
        {
            var account = await dbContext.Users.FirstOrDefaultAsync(user => user.Login == accountData.Login);
            if (account != null && passwordHasher.Verify(accountData.Password!, account.PasswordHash))
            {
                await AuthorizeUserAsync(account, httpContext);
                return true;
            }
            ErrorMessage = "Неверный логин или пароль.";
            return false;
        }

        private async Task AuthorizeUserAsync(User user, HttpContext httpContext)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserID", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var claimsIdenty = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipial = new ClaimsPrincipal(claimsIdenty);
            await httpContext.SignInAsync("Cookies", claimsPrincipial);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(user.UserId);
        }
    }
}
