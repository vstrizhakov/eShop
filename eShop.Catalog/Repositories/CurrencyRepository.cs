using eShop.Catalog.DbContexts;
using eShop.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Repositories
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
                .ToListAsync();
            return currencies;
        }

        public async Task<Currency?> GetCurrencyByIdAsync(Guid id)
        {
            var currency = await _context.Currencies
                .FindAsync(id);
            return currency;
        }
    }
}
