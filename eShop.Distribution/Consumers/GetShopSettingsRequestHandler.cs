using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetShopSettingsRequestHandler : IConsumer<GetShopSettingsRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetShopSettingsRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetShopSettingsRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                var shopSettings = _mapper.Map<ShopSettings>(distributionSettings.ShopSettings);
                var response = new GetShopSettingsResponse(accountId, shopSettings);

                await context.RespondAsync(response);
            }
        }
    }
}
