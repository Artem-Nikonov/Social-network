namespace SocialNetworkServer.Interfaces
{
    public interface IChatParticipantChecker
    {
        Task<bool> UserIsAChatParcipant(int userId, int chatId);
    }
}
