using eShopping.Distribution.Entities;

namespace eShopping.Distribution.Repositories
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> GetShopsAsync(IEnumerable<Guid> ids);
        Task CreateShopAsync(Shop currency);
        Task<IEnumerable<Shop>> GetShopsAsync();
        Task<Shop?> GetShopAsync(Guid shopId);
    }
}
