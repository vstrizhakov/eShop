using eShopping.Distribution.Exceptions;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class ViberChatUpdatedEventHandler : IConsumer<ViberChatUpdatedEvent>
    {
        private readonly IAccountService _accountService;

        public ViberChatUpdatedEventHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<ViberChatUpdatedEvent> context)
        {
            var @event = context.Message;
            try
            {
                await _accountService.UpdateViberChatAsync(@event.AccountId, @event.ViberUserId, @event.IsEnabled);
            }
            catch (AccountNotFoundException)
            {
            }
        }
    }
}
