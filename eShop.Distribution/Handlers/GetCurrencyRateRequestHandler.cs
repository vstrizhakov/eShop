using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.Handlers
{
    public class GetCurrencyRateRequestHandler : IRequestHandler<GetCurrencyRateRequest, GetCurrencyRateResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetCurrencyRateRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<GetCurrencyRateResponse> HandleRequestAsync(GetCurrencyRateRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            var preferredCurrency = distributionSettings.PreferredCurrency;

            if (preferredCurrency != null)
            {
                var currencyRates = await _distributionSettingsService.GetCurrencyRatesAsync(distributionSettings);
                var currencyRate = currencyRates.FirstOrDefault(e => e.SourceCurrencyId == request.CurrencyId);
                GetCurrencyRateResponse response;

                if (currencyRate == null)
                {
                    response = new GetCurrencyRateResponse(accountId, false, null, null);
                }
                else
                {
                    var mappedPreferredCurrency = _mapper.Map<Currency>(preferredCurrency);
                    var mappedCurrencyRate = _mapper.Map<CurrencyRate>(currencyRate);
                    response = new GetCurrencyRateResponse(accountId, true, mappedPreferredCurrency, mappedCurrencyRate);
                }

                return response;
            }

            return null;
        }
    }

}
