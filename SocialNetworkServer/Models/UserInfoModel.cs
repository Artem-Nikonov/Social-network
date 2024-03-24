using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.Models
{
    //Модель, содержащая основные данные пользователя
    public class UserInfoModel
    {
        //[BindNever]
        public int UserId { get; set; }

        //[Required(ErrorMessage ="Введите имя.")]
        //[MinLength(2, ErrorMessage ="Длина имени должна быть не меньше 2-х символов.")]
        //[Display(Name ="Имя:")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Введите Фамилию.")]
        //[MinLength(2, ErrorMessage = "Длина фамилии должна быть не меньше 2-х символов.")]
        //[Display(Name = "Фамилия:")]
        public string UserSurname { get; set; }

        public UserInfoModel() { }
        public UserInfoModel(int userId, string userName, string userSurname)
        {
            UserId = userId;
            UserName = userName;
            UserSurname = userSurname;
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
