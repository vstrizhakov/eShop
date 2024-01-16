using eShopping.Catalog.Entities;
using eShopping.Catalog.Repositories;

namespace eShopping.Catalog.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public async Task<Shop?> GetShopAsync(Guid id)
        {
            var shop = await _shopRepository.GetShopAsync(id);
            return shop;
        }

        public async Task<IEnumerable<Shop>> GetShopsAsync()
        {
            var shops = await _shopRepository.GetShopsAsync();
            return shops;
        }
    }
}
