using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.Handlers
{
    public class SetCurrencyRateRequestHandler : IRequestHandler<SetCurrencyRateRequest, SetCurrencyRateResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public SetCurrencyRateRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<SetCurrencyRateResponse> HandleRequestAsync(SetCurrencyRateRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetCurrencyRateAsync(distributionSettings, request.CurrencyId, request.Rate);
                var currencyRates = await _distributionSettingsService.GetCurrencyRatesAsync(distributionSettings);

                var mappedPreferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                var mappedCurrencyRates = _mapper.Map<IEnumerable<CurrencyRate>>(currencyRates);
                var response = new SetCurrencyRateResponse(accountId, mappedPreferredCurrency, mappedCurrencyRates);
                return response;
            }

            return null;
        }
    }

}
