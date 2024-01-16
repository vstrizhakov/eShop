using eShopping.Catalog.Entities;

namespace eShopping.Catalog.Repositories
{
    public interface IShopRepository
    {
        Task<Shop?> GetShopAsync(Guid id);
        Task<IEnumerable<Shop>> GetShopsAsync();
    }
}
