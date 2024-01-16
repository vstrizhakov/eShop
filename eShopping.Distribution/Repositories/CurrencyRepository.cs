using eShopping.Database.Extensions;
using eShopping.Distribution.DbContexts;
using eShopping.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Distribution.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly DistributionDbContext _context;

        public CurrencyRepository(DistributionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync(IEnumerable<Guid> ids)
        {
            var currencies = await _context.Currencies
                .WithDiscriminatorAsPartitionKey()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
            return currencies;
        }

        public async Task CreateCurrencyAsync(Currency currency)
        {
            _context.Currencies.Add(currency);

            await _context.SaveChangesAsync();
        }

        public async Task<Currency?> GetCurrencyAsync(Guid currencyId)
        {
            var currency = await _context.Currencies
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.Id == currencyId);

            return currency;
        }

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
        {
            var currencies = await _context.Currencies
                .WithDiscriminatorAsPartitionKey()
                .ToListAsync();

            return currencies;
        }
    }
}
