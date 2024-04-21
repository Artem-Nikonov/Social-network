using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.Models
{
    public class ChatInfoModel
    {
        [BindNever]
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        [BindNever]
        public int CreatorId { get; set; }
        public static implicit operator ChatInfoModel(Chat chat)
        {
            return new ChatInfoModel()
            {
                ChatId = chat.ChatId,
                ChatName = chat.ChatName,
                CreatorId = chat.CreatorId,
            };
        }
    }
}
