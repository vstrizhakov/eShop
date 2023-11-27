using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class SetShopSettingsFilterRequestHandler : IConsumer<SetShopSettingsFilterRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public SetShopSettingsFilterRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SetShopSettingsFilterRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetFilterShopsAsync(distributionSettings, request.Filter);
                var shopSettings = _mapper.Map<ShopSettings>(distributionSettings.ShopSettings);

                var response = new SetShopSettingsFilterResponse(accountId, shopSettings);

                await context.RespondAsync(response);
            }
        }
    }

}
