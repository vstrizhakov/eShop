using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetShopSettingsShopsRequestHandler : IConsumer<GetShopSettingsShopsRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetShopSettingsShopsRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetShopSettingsShopsRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                var shops = await _distributionSettingsService.GetShopsAsync(distributionSettings);
                var mappedShops = _mapper.Map<IEnumerable<Shop>>(shops);

                var response = new GetShopSettingsShopsResponse(accountId, mappedShops);

                await context.RespondAsync(response);
            }
        }
    }

}
