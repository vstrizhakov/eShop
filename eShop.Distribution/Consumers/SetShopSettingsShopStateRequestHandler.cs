using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class SetShopSettingsShopStateRequestHandler : IConsumer<SetShopSettingsShopStateRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public SetShopSettingsShopStateRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SetShopSettingsShopStateRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetShopIsEnabledAsync(distributionSettings, request.ShopId, request.IsEnabled);
                var shops = await _distributionSettingsService.GetShopsAsync(distributionSettings);
                var mappedShops = _mapper.Map<IEnumerable<Shop>>(shops);

                var response = new SetShopSettingsShopStateResponse(accountId, mappedShops);

                await context.RespondAsync(response);
            }
        }
    }

}
