using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;

namespace eShop.Distribution.Handlers
{
    public class GetShopSettingsShopsRequestHandler : IRequestHandler<GetShopSettingsShopsRequest, GetShopSettingsShopsResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetShopSettingsShopsRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<GetShopSettingsShopsResponse> HandleRequestAsync(GetShopSettingsShopsRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                var shops = await _distributionSettingsService.GetShopsAsync(distributionSettings);
                var mappedShops = _mapper.Map<IEnumerable<Shop>>(shops);

                var response = new GetShopSettingsShopsResponse(accountId, mappedShops);
                return response;
            }

            return null;
        }
    }

}
