using eShopping.Distribution.Entities;
using eShopping.Distribution.Repositories;

namespace eShopping.Distribution.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
        {
            var currencies = await _currencyRepository.GetCurrenciesAsync();
            return currencies;
        }

        public async Task<Currency?> GetCurrencyAsync(Guid currencyId)
        {
            var currency = await _currencyRepository.GetCurrencyAsync(currencyId);
            return currency;
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
