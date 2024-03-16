using SocialNetworkServer.Interfaces;
namespace SocialNetworkServer.Services
{
    public class PasswordHasher: IPasswordHasher
    {
        public string GenerateHash(string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            return passwordHash;
        }
        public bool Verify(string password, string passwordHash)
        {
            var isVerify = BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
            return isVerify;
        }
    }
}
