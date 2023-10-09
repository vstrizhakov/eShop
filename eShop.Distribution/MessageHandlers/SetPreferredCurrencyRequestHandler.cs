using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.MessageHandlers
{
    public class SetPreferredCurrencyRequestHandler : IMessageHandler<SetPreferredCurrencyRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public SetPreferredCurrencyRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(SetPreferredCurrencyRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetPreferredCurrencyAsync(distributionSettings, request.CurrencyId);

                var preferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                var response = new SetPreferredCurrencyResponse(accountId, true, preferredCurrency);
                _producer.Publish(response);
            }
            else
            {
                var response = new SetPreferredCurrencyResponse(accountId, false, null);
                _producer.Publish(response);
            }
        }
    }
}
