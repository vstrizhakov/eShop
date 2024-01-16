using eShopping.Catalog.DbContexts;
using eShopping.Catalog.Entities;
using eShopping.Database.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Catalog.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CatalogDbContext _context;

        public CurrencyRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task CreateCurrencyAsync(Currency currency)
        {
            _context.Currencies.Add(currency);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCurrencyAsync(Currency currency)
        {
            _context.Currencies.Remove(currency);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
        {
            var currencies = await _context.Currencies
                .WithDiscriminatorAsPartitionKey()
                .ToListAsync();
            return currencies;
        }

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync(IEnumerable<Guid> currencyIds)
        {
            var currencies = await _context.Currencies
                .WithDiscriminatorAsPartitionKey()
                .Where(e => currencyIds.Contains(e.Id))
                .ToListAsync();
            return currencies;
        }

        public async Task<Currency?> GetCurrencyByIdAsync(Guid id)
        {
            var currency = await _context.Currencies
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.Id == id);
            return currency;
        }
    }
}
