using Microsoft.AspNetCore.SignalR;

namespace eShop.Distribution.Hubs
{
    public class DistributionHub : Hub<IDistributionClient>
    {
        public async Task Subscribe(Guid distributionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, distributionId.ToString());
        }

        public async Task Unsubscribe(Guid distributionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, distributionId.ToString());
        }
    }
}
