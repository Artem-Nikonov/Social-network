namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class Page
    {
        public int PageId { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }

    public class UserPage : Page
    {
        public string UserName { get; set; }
        public string UserSurname { get; set; }
    }

    public class CommunityPage : Page
    {
        public string PageName { get; set; }
        public string Thematics { get; set; }
    }
}
