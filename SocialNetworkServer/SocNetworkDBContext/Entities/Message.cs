using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public int? ChatId { get; set; }
        [BindNever]
        public virtual Chat Chat { get; set; }

        public int UserId { get; set; }
        [BindNever]
        public virtual User User { get; set; }

        public string Content { get; set; }

        public DateTime SendingDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
