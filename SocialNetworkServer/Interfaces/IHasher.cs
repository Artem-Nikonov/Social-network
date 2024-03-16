namespace SocialNetworkServer.Interfaces
{
    public interface IPasswordHasher
    {
        string GenerateHash(string password);
        public bool Verify(string password, string passwordHash);
    }
}
