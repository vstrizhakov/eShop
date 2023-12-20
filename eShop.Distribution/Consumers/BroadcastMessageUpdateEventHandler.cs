using eShop.Distribution.Exceptions;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class BroadcastMessageUpdateEventHandler : IConsumer<BroadcastMessageUpdateEvent>
    {
        private readonly IDistributionService _distributionService;

        public BroadcastMessageUpdateEventHandler(IDistributionService distributionService)
        {
            _distributionService = distributionService;
        }

        public async Task Consume(ConsumeContext<BroadcastMessageUpdateEvent> context)
        {
            var @event = context.Message;
            try
            {
                await _distributionService.SetDistributionItemStatusAsync(@event.DistributionId, @event.AnnouncerId, @event.DistributionItemId, @event.Succeeded);
            }
            catch (DistributionRequestNotFoundException)
            {
                // TODO: Handle
            }
        }
    }
}
