namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserPage UserPage { get; set; }
        public List<CommunityPage> Communities { get; set; } = new List<CommunityPage>();
        public UserAccount(string Login, string Password)
        {
            this.Login = Login;
            this.Password = Password;
            UserPage= new UserPage();
        }

    }
}
