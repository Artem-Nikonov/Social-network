using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Interfaces
{
    public interface IJWTProvider
    {
        string GenerateToken(User user);
    }
}
