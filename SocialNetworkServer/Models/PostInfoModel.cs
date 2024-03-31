using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialNetworkServer.Extentions;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Models
{
    //Модель, содержащая основные данные о посте, для отправки клиенту
    public class PostInfoModel
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int? GroupId { get; set; }
        public string Content { get; set; }
        public string CreationDate { get; set; }

        public static implicit operator PostInfoModel(Post post)
        {
            return new PostInfoModel()
            {
                PostId = post.PostId,
                UserId = post.UserId,
                GroupId = post.GroupId,
                Content = post.Content,
                CreationDate = post.CreationDate.GetSpecialFormat()
            };
        }
    }
}
