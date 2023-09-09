using eShop.Catalog.DbContexts;
using eShop.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly CatalogDbContext _context;

        public ShopRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shop>> GetShopsAsync()
        {
            var shops = await _context.Shops.ToListAsync();
            return shops;
        }
    }
}
