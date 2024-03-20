using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        public string GroupName { get; set; }

        public string Description { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }

        public List<Post> Posts { get; set; } = new();
        public List<GroupSubscription> Subscribers { get; set; } = new();
    }
}
