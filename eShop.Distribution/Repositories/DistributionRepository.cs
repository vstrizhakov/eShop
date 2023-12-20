using eShop.Database.Extensions;
using eShop.Distribution.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Repositories
{
    public class DistributionRepository : IDistributionRepository
    {
        private readonly DistributionDbContext _context;

        public DistributionRepository(DistributionDbContext context)
        {
            _context = context;
        }

        public async Task CreateDistributionAsync(Entities.Distribution distribution)
        {
            _context.Distribution.Add(distribution);

            await _context.SaveChangesAsync();
        }

        public async Task<Entities.Distribution?> GetDistributionAsync(Guid id, Guid announcerId)
        {
            var distribution = await _context.Distribution
                .WithPartitionKey(announcerId)
                .FirstOrDefaultAsync(e => e.Id == id);
            return distribution;
        }

        public async Task UpdateDistributionAsync(Entities.Distribution item)
        {
            await _context.SaveChangesAsync();
        }
    }
}
