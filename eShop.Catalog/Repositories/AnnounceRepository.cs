using eShop.Catalog.DbContexts;
using eShop.Catalog.Entities;
using eShop.Database.Extensions;
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
            _context.Announces.Add(announce);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnnounceAsync(Announce announce)
        {
            _context.Announces.Remove(announce);

            await _context.SaveChangesAsync();
        }

        public async Task<Announce?> GetAnnounceAsync(Guid id, Guid ownerId)
        {
            var announce = await _context.Announces
                .WithPartitionKey(ownerId)
                .FirstOrDefaultAsync(e => e.Id == id);

            return announce;
        }

        public async Task<IEnumerable<Announce>> GetAnnouncesAsync(Guid ownerId)
        {
            var announces = await _context.Announces
                .WithPartitionKey(ownerId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return announces;
        }

        public async Task UpdateAnnounceAsync(Announce announce)
        {
            await _context.SaveChangesAsync();
        }
    }
}
