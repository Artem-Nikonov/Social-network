using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.AuxiliaryClasses
{
    //Модель, содержащая основные данные пользователя
    public class UserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }

        public UserInfo() { }
        public UserInfo(int userId, string userName, string userSurname)
        {
            UserId = userId;
            UserName = userName;
            UserSurname = userSurname;
        }

        public static implicit operator UserInfo(User user)
        {
            return new UserInfo(user.UserId, user.UserName, user.UserSurname);
        }

        public string GetFullName()
        {
            return $"{UserName} {UserSurname}";
        }
    }
}
