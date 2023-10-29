using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.MessageHandlers
{
    public class GetDistributionSettingsRequestHandler : IRequestHandler<GetDistributionSettingsRequest, GetDistributionSettingsResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetDistributionSettingsRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<GetDistributionSettingsResponse> HandleRequestAsync(GetDistributionSettingsRequest request)
        {
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(request.AccountId);
            if (distributionSettings != null)
            {
                var mappedDistributionSettings = _mapper.Map<DistributionSettings>(distributionSettings);
                var response = new GetDistributionSettingsResponse(request.AccountId, mappedDistributionSettings);
                return response;
            }

            return null;
        }
    }
}
