using SocialNetworkServer.AuxiliaryClasses;

namespace SocialNetworkServer.Models
{
    public class UserPageModel
    {
        public UserInfoModel userInfo { get; set; } = null!;
        public PageMetaData? metaData { get; set; }
        public UserPageModel(UserInfoModel userInfo, PageMetaData? metaData)
        {
            this.userInfo = userInfo;
            this.metaData = metaData;
        }
    }
}
