using eShopping.Catalog.Services;
using eShopping.Messaging.Contracts;
using MassTransit;

namespace eShopping.Catalog.Consumers
{
    public class BroadcastAnnounceUpdateEventHandler : IConsumer<BroadcastAnnounceUpdateEvent>
    {
        private readonly IAnnouncesService _announceService;

        public BroadcastAnnounceUpdateEventHandler(IAnnouncesService announceService)
        {
            _announceService = announceService;
        }

        public async Task Consume(ConsumeContext<BroadcastAnnounceUpdateEvent> context)
        {
            var message = context.Message;

            var announce = await _announceService.GetAnnounceAsync(message.AnnounceId, message.OwnerId);
            if (announce != null)
            {
                await _announceService.SetAnnounceDistributionIdAsync(announce, message.DistributionId);
            }
        }
    }
}
