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

        public async Task UpdateDistributionSettingsAsync(DistributionSettings distributionSettings)
        {
            distributionSettings.ModifiedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            // TODO: Check
            await _context.DistributionSettings.Entry(distributionSettings).Reference(e => e.PreferredCurrency).LoadAsync();
        }

        public async Task<DistributionSettings?> GetDistributionSettingsAsync(Guid accountId)
        {
            var distributionSettings = await _context.DistributionSettings
                .Include(e => e.PreferredCurrency)
                .Include(e => e.CurrencyRates)
                    .ThenInclude(e => e.SourceCurrency)
                .Include(e => e.ComissionSettings)
                .FirstOrDefaultAsync(e => e.AccountId == accountId);
            return distributionSettings;
        }

        public async Task<IEnumerable<CurrencyRate>> GetDefaultCurrencyRatesAsync(Guid targetCurrencyId)
        {
            var currencyRates = await _context.CurrencyRates
                .Include(e => e.SourceCurrency)
                .Where(e => e.DistributionSettingsId == null)
                .Where(e => e.TargetCurrencyId == targetCurrencyId)
                .ToListAsync();
            return currencyRates;
        }
    }
}
