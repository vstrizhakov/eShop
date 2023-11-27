using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetComissionAmountRequestHandler : IConsumer<GetComissionAmountRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;

        public GetComissionAmountRequestHandler(IDistributionSettingsService distributionSettingsService)
        {
            _distributionSettingsService = distributionSettingsService;
        }

        public async Task Consume(ConsumeContext<GetComissionAmountRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new GetComissionAmountResponse(accountId, comissionSettings.Amount);

                await context.RespondAsync(response);
            }
        }
    }
}
