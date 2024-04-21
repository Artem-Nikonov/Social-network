using SocialNetworkServer.AuxiliaryClasses;

namespace SocialNetworkServer.Interfaces
{
    public interface IPaginator
    {
        PaginationData<T> BuildPaginationDataFromPageId<T>(List<T> items, int pageId, int paginationConstant);
        PaginationData<T> BuildPaginationDataFromLastItemId<T>(List<T> items, int lastItemId, int paginationConstant);
    }
}
