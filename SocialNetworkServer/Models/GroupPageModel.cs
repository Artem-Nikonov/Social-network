using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Models
{
    public class GroupPageModel
    {
        public GroupInfoModel groupInfo { get; set; } = null!;
        public PageMetaData metaData { get; set; }
        public GroupPageModel(GroupInfoModel groupInfo, PageMetaData? metaData)
        {
            this.groupInfo = groupInfo;
            this.metaData = metaData;
        }
    }
}
