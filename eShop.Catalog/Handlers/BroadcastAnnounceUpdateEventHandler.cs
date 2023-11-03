using eShop.Catalog.Repositories;
using eShop.Catalog.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Catalog.Handlers
{
    public class BroadcastAnnounceUpdateEventHandler : IMessageHandler<BroadcastAnnounceUpdateEvent>
    {
        private readonly IAnnouncesService _announceService;

        public BroadcastAnnounceUpdateEventHandler(IAnnouncesService announceService)
        {
            _announceService = announceService;
        }

        public async Task HandleMessageAsync(BroadcastAnnounceUpdateEvent message)
        {
            var announce = await _announceService.GetAnnounceAsync(message.AnnounceId);
            if (announce != null)
            {
                await _announceService.SetAnnounceDistributionIdAsync(announce, message.DistributionId);
            }
        }
    }
}
