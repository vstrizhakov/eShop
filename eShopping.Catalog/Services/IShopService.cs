using eShopping.Catalog.Entities;

namespace eShopping.Catalog.Services
{
    public interface IShopService
    {
        Task<Shop?> GetShopAsync(Guid id);
        Task<IEnumerable<Shop>> GetShopsAsync();
    }
}
