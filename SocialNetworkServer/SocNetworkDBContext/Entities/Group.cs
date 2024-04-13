using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class Group
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }
        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        public PostPermissions PostPermissions { get; set; }

        public List<Post> Posts { get; set; } = new();
        public List<GroupSubscription> Subscribers { get; set; } = new();
    }

}
