using eShop.Distribution.DbContexts;
using eShop.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Repositories
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
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
            return currencies;
        }

        public async Task CreateCurrencyAsync(Currency currency)
        {
            _context.Currencies.Add(currency);

            await _context.SaveChangesAsync();
        }
    }
}
