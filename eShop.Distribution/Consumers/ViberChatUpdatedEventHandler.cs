using eShop.Distribution.Exceptions;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using MassTransit;

namespace eShop.Distribution.Consumers
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
