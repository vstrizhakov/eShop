using eShop.Catalog.Services;
using eShop.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace eShop.Catalog.Hubs
{
    [Authorize]
    public class AnnouncesHub : Hub<IAnnouncesHubClient>
    {
        private readonly IAnnouncesService _announcesService;

        public AnnouncesHub(IAnnouncesService announcesService)
        {
            _announcesService = announcesService;
        }

        public async Task Subscribe(SubscribeToAnnounceRequest request)
        {
            var announceId = request.AnnounceId;

            var userId = Context.User.GetAccountId();
            var announce = await _announcesService.GetAnnounceAsync(announceId);
            if (announce == null || announce.OwnerId != userId)
            {
                throw new InvalidOperationException("The specified announce doesn't exist.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, announceId.ToString());
        }

        public async Task Unsubscribe(UnsubscribeFromAnnounceRequest request)
        {
            var announceId = request.AnnounceId;

            var userId = Context.User.GetAccountId();
            var announce = await _announcesService.GetAnnounceAsync(announceId);
            if (announce == null || announce.OwnerId != userId)
            {
                throw new InvalidOperationException("The specified announce doesn't exist.");
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, announceId.ToString());
        }
    }
}
