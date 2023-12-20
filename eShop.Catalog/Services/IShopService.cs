using eShop.Catalog.Entities;

namespace eShop.Catalog.Services
{
    public interface IShopService
    {
        Task<Shop?> GetShopAsync(Guid id);
        Task<IEnumerable<Shop>> GetShopsAsync();
    }
}
