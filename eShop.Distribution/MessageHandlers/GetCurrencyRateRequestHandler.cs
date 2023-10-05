using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.MessageHandlers
{
    public class GetCurrencyRateRequestHandler : IMessageHandler<GetCurrencyRateRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public GetCurrencyRateRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(GetCurrencyRateRequest request)
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

                _producer.Publish(response);
            }
        }
    }
}
