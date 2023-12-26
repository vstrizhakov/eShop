using eShop.ExchangeRate;

namespace eShop.Distribution.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateClient _client;

        public ExchangeRateService(IExchangeRateClient client)
        {
            _client = client;
        }

        public async Task<IDictionary<string, float>> GetLatestRatesAsync(string currency)
        {
            var response = await _client.GetLatestAsync(currency);
            return response.ConversionRates;
        }
    }
}
