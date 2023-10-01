using eShop.Distribution.DbContexts;
using eShop.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Repositories
{
    public class DistributionSettingsRepository : IDistributionSettingsRepository
    {
        private readonly DistributionDbContext _context;

        public DistributionSettingsRepository(DistributionDbContext context)
        {
            _context = context;
        }

        public async Task CreateDistributionSettingsAsync(DistributionSettings distributionSettings)
        {
            _context.DistributionSettings.Add(distributionSettings);

            await _context.SaveChangesAsync();

            await _context.DistributionSettings.Entry(distributionSettings).Reference(e => e.PreferredCurrency).LoadAsync();
        }

        public async Task<DistributionSettings?> GetActiveDistributionSettingsAsync(Guid accountId)
        {
            var distributionSettings = await _context.DistributionSettings
                .Include(e => e.PreferredCurrency)
                .OrderByDescending(e => e.CreatedAt)
                .FirstOrDefaultAsync(e => e.AccountId == accountId);
            return distributionSettings;
        }
    }
}
