﻿using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Services
{
    public class RegistrationService: IRegistrationService
    {
        private SocialNetworkDBContext dbContext;
        private IPasswordHasher passwordHasher;
        public RegistrationService (SocialNetworkDBContext dBContext,IPasswordHasher passwordHasher)
        {
            this.dbContext = dBContext;
            this.passwordHasher = passwordHasher;
        }
        public async Task<bool> TryRegisterAccountAsync(UserRegistrationModel userAccount)
        {
            var account = await dbContext.Users.FirstOrDefaultAsync(user => user.Login == userAccount.Login);
            if(account == null)
            {
                await RegisterAccountAsync(userAccount);
                return true;
            }
            return false;
        }

        private async Task RegisterAccountAsync(UserRegistrationModel userAccount)
        {
            var passwordHash = passwordHasher.GenerateHash(userAccount.Password!);
            var account = new User()
            {
                UserName = userAccount.UserName,
                UserSurname = userAccount.UserSurname,
                Login = userAccount.Login,
                PasswordHash = passwordHash
            };
            await dbContext.Users.AddAsync(account);
            await dbContext.SaveChangesAsync();
        }
    }
}
