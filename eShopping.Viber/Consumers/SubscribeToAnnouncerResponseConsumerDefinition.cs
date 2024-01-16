using MassTransit;

namespace eShopping.Viber.Consumers
{
    public class SubscribeToAnnouncerResponseConsumerDefinition : ConsumerDefinition<SubscribeToAnnouncerResponseConsumer>
    {
        public SubscribeToAnnouncerResponseConsumerDefinition()
        {
            Endpoint(e => e.InstanceId = "viber");
        }
    }
}
