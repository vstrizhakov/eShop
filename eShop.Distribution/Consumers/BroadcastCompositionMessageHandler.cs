using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class BroadcastCompositionMessageHandler : IConsumer<BroadcastAnnounceMessage>
    {
        private readonly IDistributionService _distributionService;
        private readonly IMessageBuilder _messageBuilder;

        public BroadcastCompositionMessageHandler(
            IDistributionService distributionService,
            IMessageBuilder messageBuilder)
        {
            _distributionService = distributionService;
            _messageBuilder = messageBuilder;
        }

        public async Task Consume(ConsumeContext<BroadcastAnnounceMessage> context)
        {
            var message = context.Message;
            var composition = message.Announce;

            // TODO: Handle provider is absent
            var distribution = await _distributionService.CreateDistributionAsync(message.AnnouncerId, composition);

            var distributionId = distribution.Id;
            var update = new BroadcastAnnounceUpdateEvent
            {
                AnnounceId = composition.Id,
                DistributionId = distributionId,
            };

            await context.Publish(update);

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

                await context.Publish(telegramMessage);
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

                await context.Publish(viberMessage);
            }
        }
    }
}
