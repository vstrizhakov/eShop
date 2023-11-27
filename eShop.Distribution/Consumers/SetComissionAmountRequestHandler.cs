using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class SetComissionAmountRequestHandler : IConsumer<SetComissionAmountRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;

        public SetComissionAmountRequestHandler(IDistributionSettingsService distributionSettingsService)
        {
            _distributionSettingsService = distributionSettingsService;
        }

        public async Task Consume(ConsumeContext<SetComissionAmountRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetComissionAmountAsync(distributionSettings, request.Amount);
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new SetComissionAmountResponse(accountId, comissionSettings.Amount);

                await context.RespondAsync(response);
            }
        }
    }

}
