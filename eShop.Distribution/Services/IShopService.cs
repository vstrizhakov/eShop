using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IShopService
    {
        Task SyncShopsAsync(IEnumerable<Shop> shops);
    }
}
