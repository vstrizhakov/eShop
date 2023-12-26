using eShop.ExchangeRate.Models;
using Newtonsoft.Json;

namespace eShop.ExchangeRate
{
    public class ExchangeRateClient : IExchangeRateClient
    {
        private readonly ExchangeRateClientOptions _options;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public ExchangeRateClient(ExchangeRateClientOptions options, HttpClient httpClient)
        {
            _options = options;
            _httpClient = httpClient;

            if (_options.ApiKey == null)
            {
                throw new ArgumentNullException(nameof(_options.ApiKey));
            }

            _httpClient.BaseAddress = new Uri($"https://v6.exchangerate-api.com/v6/{_options.ApiKey}/");
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new SnakeCasePropertyNamesContractResolver(),
            };
        }

        public async Task<GetLatestResponse> GetLatestAsync(string currency)
        {
            var response = await _httpClient.GetAsync($"latest/{currency}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<GetLatestResponse>(content, _jsonSerializerSettings);

            return data;
        }
    }
}
