namespace SocialNetworkServer.Models
{
    internal class Subscription
    {
        public int SubscriptionId { get; set; }
        public Page Page { get; set; }
        public int SubscriberId { get; set; }
        public UserPage Subscriber { get; set; }
    }
}
