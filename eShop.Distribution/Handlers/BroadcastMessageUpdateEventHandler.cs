using eShop.Distribution.Exceptions;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.Handlers
{
    public class BroadcastMessageUpdateEventHandler : IMessageHandler<BroadcastMessageUpdateEvent>
    {
        private readonly IDistributionService _distributionService;

        public BroadcastMessageUpdateEventHandler(IDistributionService distributionService)
        {
            _distributionService = distributionService;
        }

        public async Task HandleMessageAsync(BroadcastMessageUpdateEvent message)
        {
            try
            {
                await _distributionService.UpdateDistributionRequestStatusAsync(message.RequestId, message.Succeeded);
            }
            catch (DistributionRequestNotFoundException)
            {
                // TODO: Handle
            }
        }
    }
}
