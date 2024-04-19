using SocialNetworkServer.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class Chat
    {
        public int ChatId { get; set; }

        public string ChatName { get; set; }

        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }

        public List<Message> Messages { get; set; } = new();
        public List<ChatParticipants> Participants { get; set; } = new();
    }
}
