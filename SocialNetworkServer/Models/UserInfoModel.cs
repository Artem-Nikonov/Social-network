using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Models
{
    //Модель, содержащая основные данные пользователя
    public class UserInfoModel
    {
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public string UserSurname { get; private set; }

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
