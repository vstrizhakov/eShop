using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;

namespace eShop.Distribution.MessageHandlers
{
    public class SetShopSettingsFilterRequestHandler : IMessageHandler<SetShopSettingsFilterRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public SetShopSettingsFilterRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(SetShopSettingsFilterRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetFilterShopsAsync(distributionSettings, request.Filter);
                var shopSettings = _mapper.Map<ShopSettings>(distributionSettings.ShopSettings);

                var response = new SetShopSettingsFilterResponse(accountId, shopSettings);
                _producer.Publish(response);
            }
        }
    }
}
