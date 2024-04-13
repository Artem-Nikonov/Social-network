using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SocialNetworkServer.Enums
{
    public enum PostPermissions
    {
        [Display(Name = "Только администратор")]
        AdminOnly,
        [Display(Name = "Только подписчики")]
        SubscribersOnly,
        [Display(Name = "Все")]
        Everyone
    }
}
