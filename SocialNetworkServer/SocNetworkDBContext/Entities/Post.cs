using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int? GroupId { get; set; }

        public virtual Group Group { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
    }
}
