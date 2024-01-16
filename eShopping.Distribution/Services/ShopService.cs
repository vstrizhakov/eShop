using eShopping.Distribution.Entities;
using eShopping.Distribution.Repositories;

namespace eShopping.Distribution.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public async Task SyncShopsAsync(IEnumerable<Shop> shops)
        {
            var existingShops = await _shopRepository.GetShopsAsync(shops.Select(e => e.Id));
            foreach (var shop in shops)
            {
                if (!existingShops.Any(e => e.Id == shop.Id))
                {
                    await _shopRepository.CreateShopAsync(shop);
                }
            }
        }
    }
}
