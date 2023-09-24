using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;

namespace eShop.Distribution.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task SyncCurrenciesAsync(IEnumerable<Currency> currencies)
        {
            var existingCurrencies = await _currencyRepository.GetCurrenciesAsync(currencies.Select(e => e.Id));
            foreach (var currency in currencies)
            {
                if (!existingCurrencies.Any(e => e.Id == currency.Id))
                {
                    await _currencyRepository.CreateCurrencyAsync(currency);
                }
            }
        }
    }
}
