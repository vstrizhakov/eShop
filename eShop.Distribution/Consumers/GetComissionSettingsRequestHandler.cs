using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetComissionSettingsRequestHandler : IConsumer<GetComissionSettingsRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;

        public GetComissionSettingsRequestHandler(IDistributionSettingsService distributionSettingsService)
        {
            _distributionSettingsService = distributionSettingsService;
        }

        public async Task Consume(ConsumeContext<GetComissionSettingsRequest> context)
        {
            var request = context.Message;

            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                var comissionSettings = distributionSettings.ComissionSettings;
                var response = new GetComissionSettingsResponse(accountId, comissionSettings.Amount);

                await context.RespondAsync(response);
            }
        }
    }

}
