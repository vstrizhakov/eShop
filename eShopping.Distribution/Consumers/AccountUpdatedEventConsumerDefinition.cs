using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class AccountUpdatedEventConsumerDefinition : ConsumerDefinition<AccountUpdatedEventConsumer>
    {
        public AccountUpdatedEventConsumerDefinition()
        {
            Endpoint(e => e.InstanceId = "distribution");
        }
    }
}
