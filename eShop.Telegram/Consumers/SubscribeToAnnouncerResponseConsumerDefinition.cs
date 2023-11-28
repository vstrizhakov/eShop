using MassTransit;

namespace eShop.Telegram.Consumers
{
    public class SubscribeToAnnouncerResponseConsumerDefinition : ConsumerDefinition<SubscribeToAnnouncerResponseConsumer>
    {
        public SubscribeToAnnouncerResponseConsumerDefinition()
        {
            Endpoint(e => e.InstanceId = "telegram");
        }
    }
}
