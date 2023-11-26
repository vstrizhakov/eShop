using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.Handlers
{
    public class BroadcastCompositionMessageHandler : IMessageHandler<BroadcastAnnounceMessage>
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

        public async Task HandleMessageAsync(BroadcastAnnounceMessage message)
        {
            var composition = message.Announce;

            // TODO: Handle provider is absent
            var distribution = await _distributionService.CreateDistributionAsync(message.AnnouncerId, composition);

            var distributionId = distribution.Id;
            var update = new BroadcastAnnounceUpdateEvent
            {
                AnnounceId = composition.Id,
                DistributionId = distributionId,
            };

            _producer.Publish(update);

            var distributionItems = distribution.Items.Where(e => e.Status == Entities.DistributionItemStatus.Pending);

            var telegramFormatter = new TelegramFormatter();
            var telegramRequests = distributionItems.Where(e => e.TelegramChatId.HasValue);
            foreach (var telegramRequest in telegramRequests)
            {
                var messageToSend = _messageBuilder.FromComposition(composition, telegramRequest.DistributionSettings, telegramFormatter);
                var telegramMessage = new BroadcastCompositionToTelegramMessage
                {
                    RequestId = telegramRequest.Id,
                    TargetId = telegramRequest.TelegramChatId!.Value,
                    Message = messageToSend,
                };
                _producer.Publish(telegramMessage);
            }

            var viberFormatter = new ViberFormatter();
            var viberRequests = distributionItems.Where(e => e.ViberChatId.HasValue);
            foreach (var viberRequest in viberRequests)
            {
                var messageToSend = _messageBuilder.FromComposition(composition, viberRequest.DistributionSettings, viberFormatter);
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
