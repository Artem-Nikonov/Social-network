using SocialNetworkServer.Models;

namespace SocialNetworkServer.Interfaces
{
    public interface IRegistrationService
    {
        Task<bool> TryRegisterAccountAsync(UserRegistrationModel userAccount);
    }
}
