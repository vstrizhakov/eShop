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

        public async Task HandleMessageAsync(SetComissionAmountRequest message)
        {
            var accountId = message.AccountId;
            var distributionSettings = await _distributionSettingsService.SetComissionAmountAsync(accountId, message.Amount);
            if (distributionSettings != null)
            {
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new SetComissionAmountResponse(accountId, comissionSettings.Show, comissionSettings.Amount);
                _producer.Publish(response);
            }
        }
    }
}
