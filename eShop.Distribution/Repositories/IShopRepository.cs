using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> GetShopsAsync(IEnumerable<Guid> ids);
        Task CreateShopAsync(Shop currency);
        Task<IEnumerable<Shop>> GetShopsAsync();
        Task<Shop?> GetShopAsync(Guid shopId);
    }
}
