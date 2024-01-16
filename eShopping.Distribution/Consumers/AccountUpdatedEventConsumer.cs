using eShopping.Distribution.Exceptions;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class AccountUpdatedEventConsumer : IConsumer<AccountUpdatedEvent>
    {
        private readonly IAccountService _accountService;

        public AccountUpdatedEventConsumer(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<AccountUpdatedEvent> context)
        {
            var @event = context.Message;
            try
            {
                var account = @event.Account;
                await _accountService.UpdateAccountAsync(account.Id, account.TelegramUserId, account.ViberUserId, account.FirstName, account.LastName);
            }
            catch (AccountAlreadyExistsException)
            {
            }
        }
    }
}
