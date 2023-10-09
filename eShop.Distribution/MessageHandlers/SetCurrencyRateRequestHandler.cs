using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.MessageHandlers
{
    public class SetCurrencyRateRequestHandler : IMessageHandler<SetCurrencyRateRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public SetCurrencyRateRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(SetCurrencyRateRequest message)
        {
            var accountId = message.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetCurrencyRateAsync(distributionSettings, message.CurrencyId, message.Rate);
                var currencyRates = await _distributionSettingsService.GetCurrencyRatesAsync(distributionSettings);

                var mappedPreferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                var mappedCurrencyRates = _mapper.Map<IEnumerable<CurrencyRate>>(currencyRates);
                var response = new SetCurrencyRateResponse(accountId, mappedPreferredCurrency, mappedCurrencyRates);
                _producer.Publish(response);
            }
        }
    }
}
