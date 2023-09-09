using eShop.Catalog.Entities;

namespace eShop.Catalog.Repositories
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> GetShopsAsync();
    }
}
