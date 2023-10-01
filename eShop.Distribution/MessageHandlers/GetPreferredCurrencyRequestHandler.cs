using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.MessageHandlers
{
    public class GetPreferredCurrencyRequestHandler : IMessageHandler<GetPreferredCurrencyRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public GetPreferredCurrencyRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(GetPreferredCurrencyRequest message)
        {
            var accountId = message.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            var preferredCurrency = _mapper.Map<Messaging.Models.Currency>(distributionSettings.PreferredCurrency);
            var response = new GetPreferredCurrencyResponse(accountId, preferredCurrency);
            _producer.Publish(response);
        }
    }
}
