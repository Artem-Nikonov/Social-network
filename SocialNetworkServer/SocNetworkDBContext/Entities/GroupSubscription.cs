namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class GroupSubscription
    {
        public int SubscriberId { get; set; }

        public int SubscribedToGroupId { get; set; }

        public User Subscriber { get; set; }
        public Group SubscribedToGroup { get; set; }
    }
}
