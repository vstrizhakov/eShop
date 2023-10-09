using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;

namespace eShop.Distribution.MessageHandlers
{
    public class GetShopSettingsRequestHandler : IMessageHandler<GetShopSettingsRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;
        
        public GetShopSettingsRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(GetShopSettingsRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                var shopSettings = _mapper.Map<ShopSettings>(distributionSettings.ShopSettings);

                var response = new GetShopSettingsResponse(accountId, shopSettings);
                _producer.Publish(response);
            }
        }
    }
}
