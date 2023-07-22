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
            var messageToSend = _messageBuilder.FromComposition(composition);

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
            if (telegramRequests.Any())
            {
                var telegramMessage = new BroadcastCompositionToTelegramMessage
                {
                    Requests = telegramRequests.Select(e => new DistributionRequest
                    {
                        RequestId = e.Id,
                        TargetId = e.TelegramChatId.Value,
                    }),
                    Message = messageToSend,
                };
                _producer.Publish(telegramMessage);
            }

            var viberRequests = distribution.Items.Where(e => e.ViberChatId.HasValue);
            if (viberRequests.Any())
            {
                var viberMessage = new BroadcastCompositionToViberMessage
                {
                    Requests = viberRequests.Select(e => new DistributionRequest
                    {
                        RequestId = e.Id,
                        TargetId = e.ViberChatId.Value,
                    }),
                    Message = messageToSend,
                };
                _producer.Publish(viberMessage);
            }
        }
    }
}
