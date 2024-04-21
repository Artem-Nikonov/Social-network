using SocialNetworkServer.Interfaces;

namespace SocialNetworkServer.AuxiliaryClasses
{
    public class PaginationService:IPaginator
    {
        public PaginationData<T> BuildPaginationDataFromPageId<T>(List<T> items, int pageId, int paginationConstant)
        {
            var paginationMeta = new PaginationMeta()
            {
                PageId = pageId,
                IsLastPage = items.Count < paginationConstant
            };
            var Data = new PaginationData<T>(items, paginationMeta);
            return Data;
        }

        public PaginationData<T> BuildPaginationDataFromLastItemId<T>(List<T> items, int lastItemId, int paginationConstant)
        {
            var paginationMeta = new PaginationMeta()
            {
                LastItemId = lastItemId,
                IsLastPage = items.Count < paginationConstant
            };
            var Data = new PaginationData<T>(items, paginationMeta);
            return Data;
        }
    }

    public class PaginationData<T>
    {
        public List<T> Items { get; set; }
        public PaginationMeta Meta { get; set; }
        public PaginationData(List<T> items, PaginationMeta meta)
        {
            Items = items;
            Meta = meta;
        }
    }

    public class PaginationMeta
    {
        public int? PageId { get; set; }
        public int? LastItemId { get; set; }
        public bool IsLastPage { get; set; }
    }
}
