namespace SocialNetworkServer.AuxiliaryClasses
{
    public class PageMetaData
    {
        public int VisitorId { get; set; }
        public bool VisitorIsOwner { get; set; }
        public PageMetaData(int VisitorId, bool VisitorIsOwner)
        {
            this.VisitorId = VisitorId;
            this.VisitorIsOwner = VisitorIsOwner;
        }
    }
}
