using eShop.Distribution.Exceptions;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class AccountRegisteredEventConsumer : IConsumer<AccountRegisteredEvent>
    {
        private readonly IAccountService _accountService;

        public AccountRegisteredEventConsumer(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<AccountRegisteredEvent> context)
        {
            var @event = context.Message;
            try
            {
                var account = @event.Account;
                await _accountService.CreateAccountAsync(account.Id, account.TelegramUserId, account.ViberUserId, account.FirstName, account.LastName, @event.AnnouncerId);
            }
            catch (AccountAlreadyExistsException)
            {
            }
        }
    }
}
