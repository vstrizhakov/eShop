using eShopping.Database.Extensions;
using eShopping.Distribution.DbContexts;
using eShopping.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Distribution.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly DistributionDbContext _context;

        public ShopRepository(DistributionDbContext context)
        {
            _context = context;
        }

        public async Task CreateShopAsync(Shop currency)
        {
            _context.Shops.Add(currency);

            await _context.SaveChangesAsync();
        }

        public async Task<Shop?> GetShopAsync(Guid shopId)
        {
            var shop = await _context.Shops
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.Id == shopId);
            return shop;
        }

        public async Task<IEnumerable<Shop>> GetShopsAsync(IEnumerable<Guid> ids)
        {
            var shops = await _context.Shops
                .WithDiscriminatorAsPartitionKey()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
            return shops;
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
