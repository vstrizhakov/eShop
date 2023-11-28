using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class AccountRegisteredEventConsumerDefinition : ConsumerDefinition<AccountRegisteredEventConsumer>
    {
        public AccountRegisteredEventConsumerDefinition()
        {
            Endpoint(e => e.InstanceId = "distribution");
        }
    }
}
