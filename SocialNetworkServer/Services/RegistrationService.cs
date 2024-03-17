using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Services
{
    public class RegistrationService
    {
        private SocialNetworkDBContext dbContext;
        private IPasswordHasher passwordHasher;
        public string? ErrorMessage;
        public RegistrationService (SocialNetworkDBContext dBContext,IPasswordHasher passwordHasher)
        {
            this.dbContext = dBContext;
            this.passwordHasher = passwordHasher;
        }
        public async Task<bool> TryRegisterAccountAsync(UserRegistrationModel userAccount)
        {
            var account = await dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Login == userAccount.Login);
            if(account == null)
            {
                await RegisterAccountAsync(userAccount);
                return true;
            }
            ErrorMessage = "Логин занят.";
            return false;
        }

        private async Task RegisterAccountAsync(UserRegistrationModel userAccount)
        {
            var login = userAccount.Login;
            var password = passwordHasher.GenerateHash(userAccount.Password!);
            var account = new UserAccount(login, password);
            await dbContext.Accounts.AddAsync(account);
            await dbContext.SaveChangesAsync();
        }
    }
}
