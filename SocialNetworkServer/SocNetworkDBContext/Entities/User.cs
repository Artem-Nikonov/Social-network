using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Post> Posts { get; set; } = new();
        // Навигационные свойства для подписчиков пользователя
        public List<UserSubscription> Followers { get; set; } = new();

        // Навигационные свойства для пользователей, на которых он подписан
        public List<UserSubscription> Followings { get; set; } = new();

        // Навигационные свойства для групп, на которые он подписан
        public List<GroupSubscription> SubscribedGroups { get; set; } = new();
    }
}
