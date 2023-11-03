using eShop.Catalog.DbContexts;
using eShop.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Repositories
{
    public class AnnounceRepository : IAnnounceRepository
    {
        private readonly CatalogDbContext _context;

        public AnnounceRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task CreateAnnounceAsync(Announce announce)
        {
            // TODO: find another way to achieve currency property on product prices exist
            var currencyIds = announce.Products
                .SelectMany(product => product.Prices.Select(price => price.CurrencyId))
                .Distinct();

            await _context.Currencies
                .Where(currency => currencyIds.Contains(currency.Id))
                .ToListAsync();

            await _context.Shops.FirstOrDefaultAsync(shop => shop.Id == announce.ShopId);

            _context.Announces.Add(announce);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnnounceAsync(Announce announce)
        {
            _context.Announces.Remove(announce);

            await _context.SaveChangesAsync();
        }

        public async Task<Announce?> GetAnnounceByIdAsync(Guid id)
        {
            var announce = await _context.Announces
                .Include(e => e.Products)
                    .ThenInclude(e => e.Prices)
                        .ThenInclude(e => e.Currency)
                .Include(e => e.Shop)
                .Include(e => e.Images)
                .FirstOrDefaultAsync(e => e.Id == id);

            return announce;
        }

        public async Task<IEnumerable<Announce>> GetAnnouncesAsync(Guid ownerId)
        {
            var announces = await _context.Announces
                .Include(e => e.Products)
                    .ThenInclude(e => e.Prices)
                        .ThenInclude(e => e.Currency)
                .Include(e => e.Shop)
                .Include(e => e.Images)
                .Where(e => e.OwnerId == ownerId)
                .OrderBy(e => e.CreatedAt)
                .ToListAsync();

            return announces;
        }

        public async Task UpdateAnnounceAsync(Announce announce)
        {
            await _context.SaveChangesAsync();
        }
    }
}
