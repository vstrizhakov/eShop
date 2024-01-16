using MassTransit;

namespace eShopping.Identity.Consumers
{
    public class AccountRegisteredEventConsumerDefinition : ConsumerDefinition<AccountRegisteredEventConsumer>
    {
        public AccountRegisteredEventConsumerDefinition()
        {
            Endpoint(e => e.InstanceId = "identity");
        }
    }
}
