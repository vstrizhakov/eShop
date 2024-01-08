namespace eShop.Distribution.Services
{
    public class DefaultCurrencyRateSyncService : IDefaultCurrencyRateSyncService
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IDefaultCurrencyRateService _defaultCurrencyRateService;
        private readonly ICurrencyService _currencyService;

        public DefaultCurrencyRateSyncService(
            IExchangeRateService exchangeRateService,
            IDefaultCurrencyRateService defaultCurrencyRateService,
            ICurrencyService currencyService)
        {
            _exchangeRateService = exchangeRateService;
            _defaultCurrencyRateService = defaultCurrencyRateService;
            _currencyService = currencyService;
        }

        public async Task SyncAsync()
        {
            var currencies = await _currencyService.GetCurrenciesAsync();
            var existingCurrencyRates = await _defaultCurrencyRateService.GetAllAsync();
            foreach (var sourceCurrency in currencies)
            {
                var currencyRates = await _exchangeRateService.GetLatestRatesAsync(sourceCurrency.Name);

                var targetCurrencies = currencies.Where(e => e != sourceCurrency);
                foreach (var targetCurrency in targetCurrencies)
                {
                    if (currencyRates.TryGetValue(targetCurrency.Name, out var currencyRate))
                    {
                        var existingCurrencyRate = existingCurrencyRates
                            .FirstOrDefault(e => e.TargetCurrency.Id == targetCurrency.Id && e.SourceCurrency.Id == sourceCurrency.Id);
                        if (existingCurrencyRate == null)
                        {
                            var newCurrencyRate = new Entities.DefaultCurrencyRate
                            {
                                TargetCurrency = targetCurrency.GeneratedEmbedded(),
                                SourceCurrency = sourceCurrency.GeneratedEmbedded(),
                                Rate = currencyRate,
                            };

                            await _defaultCurrencyRateService.AddAsync(newCurrencyRate);
                        }
                        else
                        {
                            existingCurrencyRate.Rate = currencyRate;

                            await _defaultCurrencyRateService.UpdateAsync(existingCurrencyRate);
                        }
                    }
                }
            }
        }
    }
}
