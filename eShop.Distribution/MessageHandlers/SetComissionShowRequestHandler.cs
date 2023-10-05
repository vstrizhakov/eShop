using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.MessageHandlers
{
    public class SetComissionShowRequestHandler : IMessageHandler<SetComissionShowRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IProducer _producer;

        public SetComissionShowRequestHandler(IDistributionSettingsService distributionSettingsService, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _producer = producer;
        }

        public async Task HandleMessageAsync(SetComissionShowRequest message)
        {
            var accountId = message.AccountId;
            var distributionSettings = await _distributionSettingsService.SetComissionShowAsync(accountId, message.Show);
            if (distributionSettings != null)
            {
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new SetComissionShowResponse(accountId, comissionSettings.Show, comissionSettings.Amount);
                _producer.Publish(response);
            }
        }
    }
}
