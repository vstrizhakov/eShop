using MassTransit;

namespace eShopping.Telegram.Consumers
{
    public class SubscribeToAnnouncerResponseConsumerDefinition : ConsumerDefinition<SubscribeToAnnouncerResponseConsumer>
    {
        public SubscribeToAnnouncerResponseConsumerDefinition()
        {
            Endpoint(e => e.InstanceId = "telegram");
        }
    }
}
