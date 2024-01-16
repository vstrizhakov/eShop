using eShopping.Distribution.Entities;
using eShopping.Distribution.Repositories;

namespace eShopping.Distribution.Services
{
    public class DefaultCurrencyRateService : IDefaultCurrencyRateService
    {
        private readonly IDefaultCurrencyRateRepository _defaultCurrencyRateRepository;

        public DefaultCurrencyRateService(IDefaultCurrencyRateRepository defaultCurrencyRateRepository)
        {
            _defaultCurrencyRateRepository = defaultCurrencyRateRepository;
        }

        public async Task AddAsync(DefaultCurrencyRate defaultCurrencyRates)
        {
            await _defaultCurrencyRateRepository.AddCurrencyRateAsync(defaultCurrencyRates);
        }

        public async Task<IEnumerable<DefaultCurrencyRate>> GetAllAsync()
        {
            var currencyRates = await _defaultCurrencyRateRepository.GetCurrencyRatesAsync();
            return currencyRates;
        }

        public async Task RemoveAsync(DefaultCurrencyRate defaultCurrencyRates)
        {
            await _defaultCurrencyRateRepository.RemoveCurrencyRateAsync(defaultCurrencyRates);
        }

        public async Task UpdateAsync(DefaultCurrencyRate defaultCurrencyRates)
        {
            await _defaultCurrencyRateRepository.UpdateCurrencyRateAsync(defaultCurrencyRates);
        }
    }
}
