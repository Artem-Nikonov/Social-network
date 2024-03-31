using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using SocialNetworkServer.OptionModels;

namespace SocialNetworkServer.Services
{
    public class AuthenticationService
    {
        private SocialNetworkDBContext dbContext;
        private IPasswordHasher passwordHasher;
        private IJWTProvider JwtProvider;
        public string? ErrorMessage { get; private set; }

        public AuthenticationService(SocialNetworkDBContext dBContext, IPasswordHasher passwordHasher, IJWTProvider JwtProvider)
        {
            this.dbContext = dBContext;
            this.passwordHasher = passwordHasher;
            this.JwtProvider = JwtProvider;
        }

        //Попытка аутентифицировать пользователя
        public async Task<bool> TryAuthenticateUserAsync(UserAuthorizationModel accountData, HttpContext httpContext)
        {
            var account = await dbContext.Users.FirstOrDefaultAsync(user => user.Login == accountData.Login);
            if (account != null && passwordHasher.Verify(accountData.Password!, account.PasswordHash))
            {
                AuthenticateUser(account, httpContext);
                return true;
            }
            ErrorMessage = "Неверный логин или пароль.";
            return false;
        }

        //выход из аккаунта
        public async Task LogOut(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("a_c");
        }

        //аутентификация пользователя
        private void AuthenticateUser(User user, HttpContext httpContext)
        {
            var token = JwtProvider.GenerateToken(user);
            httpContext.Response.Cookies.Append("a_c", token, new CookieOptions()
            {
                MaxAge = TimeSpan.FromHours(JWTOptions.ExpiresHours)
            });
        }

    }
}
