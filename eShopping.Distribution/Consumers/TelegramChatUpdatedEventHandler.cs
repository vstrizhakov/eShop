using eShopping.Distribution.Exceptions;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class TelegramChatUpdatedEventHandler : IConsumer<TelegramChatUpdatedEvent>
    {
        private readonly IAccountService _accountService;

        public TelegramChatUpdatedEventHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<TelegramChatUpdatedEvent> context)
        {
            var @event = context.Message;
            try
            {
                await _accountService.UpdateTelegramChatAsync(@event.AccountId, @event.TelegramChatId, @event.IsEnabled);
            }
            catch (AccountNotFoundException)
            {
            }
        }
    }
}
