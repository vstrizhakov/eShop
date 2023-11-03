using eShop.Distribution.DbContexts;
using eShop.Distribution.Entities;
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

        public async Task<Entities.Distribution?> GetDistributionByIdAsync(Guid id)
        {
            var distribution = await _context.Distribution
                .Include(e => e.Items)
                    .ThenInclude(e => e.Account)
                .FirstOrDefaultAsync(e => e.Id == id);
            return distribution;
        }

        public async Task<DistributionItem?> GetDistributionRequestAsync(Guid distributionRequestId)
        {
            var distributionRequest = await _context.DistributionItems
                .FirstOrDefaultAsync(e => e.Id == distributionRequestId);
            return distributionRequest;
        }

        public async Task UpdateDistributionItemAsync(DistributionItem item)
        {
            await _context.SaveChangesAsync();
        }
    }
}
