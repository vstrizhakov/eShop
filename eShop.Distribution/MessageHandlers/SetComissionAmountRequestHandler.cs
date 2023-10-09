using eShop.Distribution.Services;
using eShop.Messaging.Models.Distribution;
using eShop.Messaging;

namespace eShop.Distribution.MessageHandlers
{
    public class SetComissionAmountRequestHandler : IMessageHandler<SetComissionAmountRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IProducer _producer;

        public SetComissionAmountRequestHandler(IDistributionSettingsService distributionSettingsService, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _producer = producer;
        }

        public async Task HandleMessageAsync(SetComissionAmountRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetComissionAmountAsync(distributionSettings, request.Amount);
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new SetComissionAmountResponse(accountId, comissionSettings.Show, comissionSettings.Amount);
                _producer.Publish(response);
            }
        }
    }
}
