using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public int UserId { get; set; }
        [BindNever]
        public virtual User User { get; set; }

        public int? GroupId { get; set; }
        [BindNever]
        public virtual Group Group { get; set; }

        public string Content { get; set; }

        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
