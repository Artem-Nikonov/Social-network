﻿using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

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

        //Попытка аутентифицировать пользователя
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

        //выход из аккаунта
        public async Task LogOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        //аутентификация пользователя
        private async Task AuthorizeUserAsync(User user, HttpContext httpContext)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var claimsIdenty = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipial = new ClaimsPrincipal(claimsIdenty);
            await httpContext.SignInAsync("Cookies", claimsPrincipial);
        }

    }
}
