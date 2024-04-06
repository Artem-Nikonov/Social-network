using Microsoft.EntityFrameworkCore;
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

    public enum PostPermissions
    {
        [Display(Name ="Только администратор")]
        AdminOnly,
        [Display(Name = "Только подписчики")]
        SubscribersOnly,
        [Display(Name = "Все")]
        Everyone
    }
}
