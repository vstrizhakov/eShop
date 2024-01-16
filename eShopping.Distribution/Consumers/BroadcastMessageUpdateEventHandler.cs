using eShopping.Distribution.Exceptions;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using MassTransit;

namespace eShopping.Distribution.Consumers
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
