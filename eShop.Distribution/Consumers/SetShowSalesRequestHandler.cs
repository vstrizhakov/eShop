using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class SetShowSalesRequestHandler : IConsumer<SetShowSalesRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public SetShowSalesRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SetShowSalesRequest> context)
        {
            var request = context.Message;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(request.AccountId);
            if (distributionSettings != null)
            {
                await _distributionSettingsService.SetShowSalesAsync(distributionSettings, request.ShowSales);

                var mappedDistributionSettings = _mapper.Map<DistributionSettings>(distributionSettings);
                var response = new SetShowSalesResponse(request.AccountId, mappedDistributionSettings);

                await context.RespondAsync(response);
            }
        }
    }
}
