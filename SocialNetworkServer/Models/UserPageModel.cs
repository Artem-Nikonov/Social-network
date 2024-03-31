using SocialNetworkServer.AuxiliaryClasses;

namespace SocialNetworkServer.Models
{
    public class UserPageModel
    {
        public UserInfo userInfo { get; set; } = null!;
        public PageMetaData? metaData { get; set; }
    }
}
