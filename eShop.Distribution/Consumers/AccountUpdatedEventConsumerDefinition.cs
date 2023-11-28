using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class AccountUpdatedEventConsumerDefinition : ConsumerDefinition<AccountUpdatedEventConsumer>
    {
        public AccountUpdatedEventConsumerDefinition()
        {
            Endpoint(e => e.InstanceId = "distribution");
        }
    }
}
