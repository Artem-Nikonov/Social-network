namespace SocialNetworkServer.Interfaces
{
    public interface ISubscribeChecker
    {
        Task<bool> UserIsSubscribeToGroup(int userId, int groupId);
    }
}
