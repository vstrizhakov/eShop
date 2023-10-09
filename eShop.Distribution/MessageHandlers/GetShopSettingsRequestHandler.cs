using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;

namespace eShop.Distribution.MessageHandlers
{
    public class GetShopSettingsRequestHandler : IRequestHandler<GetShopSettingsRequest, GetShopSettingsResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetShopSettingsRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<GetShopSettingsResponse> HandleRequestAsync(GetShopSettingsRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                var shopSettings = _mapper.Map<ShopSettings>(distributionSettings.ShopSettings);
                var response = new GetShopSettingsResponse(accountId, shopSettings);
                return response;
            }

            return null;
        }
    }
}
