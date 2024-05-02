using SocialNetworkServer.AuxiliaryClasses;

namespace SocialNetworkServer.Models
{
    public class ChatFullInfoModel
    {
        public ChatInfoModel chatInfo { get; set; } = null!;
        public PageMetaData metaData { get; set; }

        public ChatFullInfoModel(ChatInfoModel chatInfo, PageMetaData metaData)
        {
            this.chatInfo = chatInfo;
            this.metaData = metaData;
        }
    }
}
