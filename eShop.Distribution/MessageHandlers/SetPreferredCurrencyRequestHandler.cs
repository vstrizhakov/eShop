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

        public async Task HandleMessageAsync(SetPreferredCurrencyRequest message)
        {
            var accountId = message.AccountId;

            var distributionSettings = default(Entities.DistributionSettings);

            var succeeded = true;
            try
            {
                distributionSettings = await _distributionSettingsService.SetPreferredCurrencyAsync(accountId, message.CurrencyId);

            }
            catch
            {
                succeeded = false;
            }

            var preferredCurrency = _mapper.Map<Currency>(distributionSettings?.PreferredCurrency);
            var response = new SetPreferredCurrencyResponse(accountId, succeeded, preferredCurrency);
            _producer.Publish(response);
        }
    }
}
