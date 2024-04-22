using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using SocialNetworkServer.Extentions;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.Models
{
    public class MessageInfoModel
    {
        public int MessageId { get; set; }
        public int? ChatId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string SendingDate { get; set; }
        public bool IsDeleted { get; set; }

        public MessageInfoModel() { }
        public MessageInfoModel(int? chatId, int userId, string content)
        {
            ChatId = chatId;
            UserId = userId;
            Content = content;
        }

        public static implicit operator MessageInfoModel(Message message)
        {
            return new MessageInfoModel()
            {
                MessageId = message.MessageId,
                ChatId = message.ChatId,
                UserId = message.UserId,
                Content = message.Content,
                SendingDate = message.SendingDate.GetSpecialFormat(),
                IsDeleted = message.IsDeleted,
            };
        }
    }
}
