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
        public string? ErrorMessage;
        public AuthorizationService(SocialNetworkDBContext dBContext, IPasswordHasher passwordHasher)
        {
            this.dbContext = dBContext;
            this.passwordHasher = passwordHasher;
        }

        public async Task<bool> TryAuthorizeUserAsync(UserAuthorizationModel accountData, HttpContext httpContext)
        {
            var account = await dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Login == accountData.Login);
            if (account != null && passwordHasher.Verify(accountData.Password!, account.Password))
            {
                await dbContext.Entry(account).Reference(acc=>acc.UserPage).LoadAsync();
                await AuthorizeUserAsync(account, httpContext);
                return true;
            }
            ErrorMessage = "Неверный логин или пароль.";
            return false;
        }

        private async Task AuthorizeUserAsync(UserAccount account, HttpContext httpContext)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserID", account.UserAccountId.ToString()),
                new Claim("UserPageID", account.UserPage.PageId.ToString())
            };
            var claimsIdenty = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipial = new ClaimsPrincipal(claimsIdenty);
            await httpContext.SignInAsync("Cookies", claimsPrincipial);

        }
    }
}
