using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class BroadcastCompositionMessageHandler : IMessageHandler<BroadcastCompositionMessage>
    {
        private readonly IProducer _producer;
        private readonly IDistributionService _distributionService;
        private readonly IMessageBuilder _messageBuilder;

        public BroadcastCompositionMessageHandler(
            IProducer producer,
            IDistributionService distributionService,
            IMessageBuilder messageBuilder)
        {
            _producer = producer;
            _distributionService = distributionService;
            _messageBuilder = messageBuilder;
        }

        public async Task HandleMessageAsync(BroadcastCompositionMessage message)
        {
            var composition = message.Composition;

            // TODO: Handle provider is absent
            var distribution = await _distributionService.CreateDistributionFromProviderIdAsync(message.ProviderId);

            var distributionId = distribution.Id;
            var update = new BroadcastCompositionUpdateEvent
            {
                CompositionId = composition.Id,
                DistributionGroupId = distributionId,
            };

            _producer.Publish(update);

            var telegramRequests = distribution.Items.Where(e => e.TelegramChatId.HasValue);
            foreach (var telegramRequest in telegramRequests)
            {
                var messageToSend = _messageBuilder.FromComposition(composition, telegramRequest.DistributionSettings);
                var telegramMessage = new BroadcastCompositionToTelegramMessage
                {
                    RequestId = telegramRequest.Id,
                    TargetId = telegramRequest.TelegramChatId!.Value,
                    Message = messageToSend,
                };
                _producer.Publish(telegramMessage);
            }

            var viberRequests = distribution.Items.Where(e => e.ViberChatId.HasValue);
            foreach (var viberRequest in viberRequests)
            {
                var messageToSend = _messageBuilder.FromComposition(composition, viberRequest.DistributionSettings);
                var viberMessage = new BroadcastCompositionToViberMessage
                {
                    RequestId = viberRequest.Id,
                    TargetId = viberRequest.ViberChatId!.Value,
                    Message = messageToSend,
                };
                _producer.Publish(viberMessage);
            }
        }
    }
}
