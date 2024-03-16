using SocialNetworkServer.Interfaces;

namespace SocialNetworkServer.Services
{
    public class RegistrationService
    {
        private IPasswordHasher? passwordHasher;
        public RegistrationService (IPasswordHasher passwordHasher)
        {
            this.passwordHasher = passwordHasher;
        }
        public string reg(string pass)
        {
            return passwordHasher.GenerateHash(pass);
        }
    }
}
