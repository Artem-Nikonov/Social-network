namespace SocialNetworkServer.Models
{
    internal class Post
    {
        public int PostId { get; set; }
        public int PageId { get; set; }
        public Page Page { get; set; }
        public DateTime PostDate { get; set; }
        public string PostText { get; set; }
    }
}
