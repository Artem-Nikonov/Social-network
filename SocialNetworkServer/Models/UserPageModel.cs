using SocialNetworkServer.AuxiliaryClasses;

namespace SocialNetworkServer.Models
{
    public class UserPageModel
    {
        public UserInfo userInfo { get; set; } = null!;
        public PageMetaData? metaData { get; set; }
        public UserPageModel(UserInfo userInfo, PageMetaData? metaData)
        {
            this.userInfo = userInfo;
            this.metaData = metaData;
        }
    }
}
