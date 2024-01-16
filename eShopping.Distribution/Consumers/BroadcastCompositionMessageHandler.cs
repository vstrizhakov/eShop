using eShopping.Distribution.Entities;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using MassTransit;

namespace eShopping.Distribution.Consumers
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

            // TODO: Handle provider/announcer is absent
            var distribution = await _distributionService.CreateDistributionAsync(message.AnnouncerId, composition);

            var distributionId = distribution.Id;
            var update = new BroadcastAnnounceUpdateEvent
            {
                AnnounceId = composition.Id,
                OwnerId = message.AnnouncerId, // TODO: check whether taking message.AnnouncerId is valid here
                DistributionId = distributionId,
            };

            await context.Publish(update);

            var telegramFormatter = new TelegramFormatter();
            var viberFormatter = new ViberFormatter();
            foreach (var target in distribution.Targets)
            {
                var distributionItems = target.Items.Where(e => e.Status == DistributionItemStatus.Pending);

                var telegramRequests = distributionItems.Where(e => e.TelegramChatId.HasValue);
                foreach (var telegramRequest in telegramRequests)
                {
                    var messageToSend = _messageBuilder.FromComposition(composition, target.DistributionSettings, telegramFormatter);
                    var telegramMessage = new BroadcastCompositionToTelegramMessage
                    {
                        DistributionId = distribution.Id,
                        AnnouncerId = distribution.AnnouncerId,
                        DistributionItemId = telegramRequest.Id,
                        TargetId = telegramRequest.TelegramChatId!.Value,
                        Message = messageToSend,
                    };

                    await context.Publish(telegramMessage);
                }

                var viberRequests = distributionItems.Where(e => e.ViberChatId.HasValue);
                foreach (var viberRequest in viberRequests)
                {
                    var messageToSend = _messageBuilder.FromComposition(composition, target.DistributionSettings, viberFormatter);
                    var viberMessage = new BroadcastCompositionToViberMessage
                    {
                        DistributionId = distribution.Id,
                        AnnouncerId = distribution.AnnouncerId,
                        DistributionItemId = viberRequest.Id,
                        TargetId = viberRequest.ViberChatId!.Value,
                        Message = messageToSend,
                    };

                    await context.Publish(viberMessage);
                }
            }
        }
    }
}
