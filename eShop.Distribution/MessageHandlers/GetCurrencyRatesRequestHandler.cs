using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.MessageHandlers
{
    public class GetCurrencyRatesRequestHandler : IMessageHandler<GetCurrencyRatesRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public GetCurrencyRatesRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(GetCurrencyRatesRequest message)
        {
            var accountId = message.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings.PreferredCurrencyId != null)
            {
                var currencyRates = await _distributionSettingsService.GetCurrencyRatesAsync(distributionSettings);

                var mappedPreferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                var mappedCurrencyRates = _mapper.Map<IEnumerable<CurrencyRate>>(currencyRates);
                var response = new GetCurrencyRatesResponse(accountId, mappedPreferredCurrency, mappedCurrencyRates);
                _producer.Publish(response);
            }
        }
    }
}
