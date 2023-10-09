using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;

namespace eShop.Distribution.MessageHandlers
{
    public class SetShopSettingsFilterRequestHandler : IRequestHandler<SetShopSettingsFilterRequest, SetShopSettingsFilterResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public SetShopSettingsFilterRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<SetShopSettingsFilterResponse> HandleRequestAsync(SetShopSettingsFilterRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetFilterShopsAsync(distributionSettings, request.Filter);
                var shopSettings = _mapper.Map<ShopSettings>(distributionSettings.ShopSettings);

                var response = new SetShopSettingsFilterResponse(accountId, shopSettings);
                return response;
            }

            return null;
        }
    }

}
