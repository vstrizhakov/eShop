using eShop.Catalog.DbContexts;
using eShop.Catalog.Entities;
using eShop.Database.Extensions;
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

        public async Task<Shop?> GetShopAsync(Guid id)
        {
            var shop = await _context.Shops
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.Id == id);
            return shop;
        }

        public async Task<IEnumerable<Shop>> GetShopsAsync()
        {
            var shops = await _context.Shops
                .WithDiscriminatorAsPartitionKey()
                .ToListAsync();
            return shops;
        }
    }
}
