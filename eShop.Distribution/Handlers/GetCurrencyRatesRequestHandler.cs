using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.Handlers
{
    public class GetCurrencyRatesRequestHandler : IRequestHandler<GetCurrencyRatesRequest, GetCurrencyRatesResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetCurrencyRatesRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<GetCurrencyRatesResponse> HandleRequestAsync(GetCurrencyRatesRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings.PreferredCurrencyId != null)
            {
                var currencyRates = await _distributionSettingsService.GetCurrencyRatesAsync(distributionSettings);
                var mappedPreferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                var mappedCurrencyRates = _mapper.Map<IEnumerable<CurrencyRate>>(currencyRates);

                var response = new GetCurrencyRatesResponse(accountId, mappedPreferredCurrency, mappedCurrencyRates);
                return response;
            }

            return null;
        }
    }

}
