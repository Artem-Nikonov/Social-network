using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.Models
{
    //Модель, содержащая основные данные пользователя
    public class UserInfoModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }

        public UserInfoModel() { }
        public UserInfoModel(int userId, string userName, string userSurname)
        {
            UserId = userId;
            UserName = userName;
            UserSurname = userSurname;
        }
        public UserInfoModel(int userId, string userFullName)
        {
            UserId = userId;
            var userInfo= userFullName.Split(' ');
            UserName = userInfo[0];
            UserSurname = userInfo[1];
        }
        public static implicit operator UserInfoModel(User user)
        {
            return new UserInfoModel(user.UserId, user.UserName, user.UserSurname);
        }

        public string GetFullName()
        {
            return $"{UserName} {UserSurname}";
        }
    }
}
