using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class UserSubscription
    {
        public int SubscriberId { get; set; }
        public User Subscriber { get; set; }
        public int SubscribedToUserId { get; set; }
        public User SubscribedToUser { get; set; }
    }
}
