using eShopping.Database.Extensions;
using eShopping.Distribution.DbContexts;
using eShopping.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Distribution.Repositories
{
    public class DefaultCurrencyRateRepository : IDefaultCurrencyRateRepository
    {
        private readonly DistributionDbContext _context;

        public DefaultCurrencyRateRepository(DistributionDbContext context)
        {
            _context = context;
        }

        public async Task AddCurrencyRateAsync(DefaultCurrencyRate defaultCurrencyRate)
        {
            _context.DefaultCurrencyRates.Add(defaultCurrencyRate);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DefaultCurrencyRate>> GetCurrencyRatesAsync()
        {
            var currencyRates = await _context.DefaultCurrencyRates
                .WithDiscriminatorAsPartitionKey()
                .ToListAsync();

            return currencyRates;
        }

        public async Task<IEnumerable<DefaultCurrencyRate>> GetCurrencyRatesAsync(Guid targetCurrencyId)
        {
            var currencyRates = await _context.DefaultCurrencyRates
                .WithDiscriminatorAsPartitionKey()
                .Where(e => e.TargetCurrency.Id == targetCurrencyId)
                .ToListAsync();

            return currencyRates;
        }

        public async Task RemoveCurrencyRateAsync(DefaultCurrencyRate defaultCurrencyRate)
        {
            _context.DefaultCurrencyRates.Remove(defaultCurrencyRate);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCurrencyRateAsync(DefaultCurrencyRate defaultCurrencyRate)
        {
            await _context.SaveChangesAsync();
        }
    }
}
