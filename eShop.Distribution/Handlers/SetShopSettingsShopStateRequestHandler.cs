using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;

namespace eShop.Distribution.Handlers
{
    public class SetShopSettingsShopStateRequestHandler : IRequestHandler<SetShopSettingsShopStateRequest, SetShopSettingsShopStateResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public SetShopSettingsShopStateRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<SetShopSettingsShopStateResponse> HandleRequestAsync(SetShopSettingsShopStateRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetShopIsEnabledAsync(distributionSettings, request.ShopId, request.IsEnabled);
                var shops = await _distributionSettingsService.GetShopsAsync(distributionSettings);
                var mappedShops = _mapper.Map<IEnumerable<Shop>>(shops);

                var response = new SetShopSettingsShopStateResponse(accountId, mappedShops);
                return response;
            }

            return null;
        }
    }

}
