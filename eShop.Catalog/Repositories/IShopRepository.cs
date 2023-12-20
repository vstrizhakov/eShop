using eShop.Catalog.Entities;

namespace eShop.Catalog.Repositories
{
    public interface IShopRepository
    {
        Task<Shop?> GetShopAsync(Guid id);
        Task<IEnumerable<Shop>> GetShopsAsync();
    }
}
