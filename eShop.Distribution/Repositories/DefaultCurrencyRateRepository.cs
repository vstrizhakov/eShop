using eShop.Database.Extensions;
using eShop.Distribution.DbContexts;
using eShop.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Repositories
{
    public class DefaultCurrencyRateRepository : IDefaultCurrencyRateRepository
    {
        private readonly DistributionDbContext _context;

        public DefaultCurrencyRateRepository(DistributionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DefaultCurrencyRate>> GetCurrencyRatesAsync(Guid targetCurrencyId)
        {
            var currencyRates = await _context.DefaultCurrencyRates
                .WithDiscriminatorAsPartitionKey()
                .Where(e => e.TargetCurrency.Id == targetCurrencyId)
                .ToListAsync();

            return currencyRates;
        }
    }
}
