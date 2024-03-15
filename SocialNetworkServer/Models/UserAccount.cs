namespace SocialNetworkServer.Models
{
    internal class UserAccount
    {
        public int UserAccountId { get; set; }
        public string Login { get; set; }
        public UserPage UserPage { get; set; }
        public List<CommunityPage> Communities { get; set; }
    }
}
