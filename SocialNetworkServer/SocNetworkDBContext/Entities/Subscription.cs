using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class UserSubscription
    {
        [Key]
        public int UserSubscriptionId { get; set; }

        [Required]
        public int SubscriberId { get; set; }

        public User Subscriber { get; set; }

        [Required]
        public int SubscribedToUserId { get; set; }

        public User SubscribedToUser { get; set; }
    }

    public class GroupSubscription
    {
        public int SubscriberId { get; set; }

        public int SubscribedToGroupId { get; set; }

        public User Subscriber { get; set; }
        public Group SubscribedToGroup { get; set; }
    }
}
