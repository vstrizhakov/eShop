using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetDistributionSettingsRequestHandler : IConsumer<GetDistributionSettingsRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetDistributionSettingsRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetDistributionSettingsRequest> context)
        {
            var request = context.Message;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(request.AccountId);
            if (distributionSettings != null)
            {
                var mappedDistributionSettings = _mapper.Map<DistributionSettings>(distributionSettings);
                var response = new GetDistributionSettingsResponse(request.AccountId, mappedDistributionSettings);

                await context.RespondAsync(response);
            }
        }
    }
}
