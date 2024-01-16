using eShopping.Distribution.Entities;

namespace eShopping.Distribution.Services
{
    public interface IShopService
    {
        Task SyncShopsAsync(IEnumerable<Shop> shops);
    }
}
