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

        public virtual User Subscriber { get; set; }

        [Required]
        public int SubscribedToUserId { get; set; }

        public virtual User SubscribedToUser { get; set; }
    }

    public class GroupSubscription
    {
        public int GroupSubscriptionId { get; set; }

        [Required]
        public int SubscriberId { get; set; }

        public User Subscriber { get; set; }

        [Required]
        public int SubscribedToGroupId { get; set; }

        public virtual Group SubscribedToGroup { get; set; }
    }
}
