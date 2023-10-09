using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;

namespace eShop.Distribution.MessageHandlers
{
    public class SetShopSettingsShopStateRequestHandler : IMessageHandler<SetShopSettingsShopStateRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public SetShopSettingsShopStateRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(SetShopSettingsShopStateRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetShopIsEnabledAsync(distributionSettings, request.ShopId, request.IsEnabled);
                var shops = await _distributionSettingsService.GetShopsAsync(distributionSettings);

                var mappedShops = _mapper.Map<IEnumerable<Shop>>(shops);
                var response = new SetShopSettingsShopStateResponse(accountId, mappedShops);
                _producer.Publish(response);
            }
        }
    }
}
