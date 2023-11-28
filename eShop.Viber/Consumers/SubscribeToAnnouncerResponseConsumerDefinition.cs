using MassTransit;

namespace eShop.Viber.Consumers
{
    public class SubscribeToAnnouncerResponseConsumerDefinition : ConsumerDefinition<SubscribeToAnnouncerResponseConsumer>
    {
        public SubscribeToAnnouncerResponseConsumerDefinition()
        {
            Endpoint(e => e.InstanceId = "viber");
        }
    }
}
