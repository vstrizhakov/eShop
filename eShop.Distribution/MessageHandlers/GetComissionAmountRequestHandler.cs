using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.MessageHandlers
{
    public class GetComissionAmountRequestHandler : IMessageHandler<GetComissionAmountRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IProducer _producer;

        public GetComissionAmountRequestHandler(IDistributionSettingsService distributionSettingsService, IProducer producer)
        {
            _distributionSettingsService = distributionSettingsService;
            _producer = producer;
        }

        public async Task HandleMessageAsync(GetComissionAmountRequest message)
        {
            var accountId = message.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new GetComissionAmountResponse(accountId, comissionSettings.Amount);
                _producer.Publish(response);
            }
        }
    }
}
