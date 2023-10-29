using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.Handlers
{
    public class SetShowSalesRequestHandler : IRequestHandler<SetShowSalesRequest, SetShowSalesResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public SetShowSalesRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<SetShowSalesResponse> HandleRequestAsync(SetShowSalesRequest request)
        {
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(request.AccountId);
            if (distributionSettings != null)
            {
                await _distributionSettingsService.SetShowSalesAsync(distributionSettings, request.ShowSales);

                var mappedDistributionSettings = _mapper.Map<DistributionSettings>(distributionSettings);
                var response = new SetShowSalesResponse(request.AccountId, mappedDistributionSettings);
                return response;
            }

            return null;
        }
    }
}
