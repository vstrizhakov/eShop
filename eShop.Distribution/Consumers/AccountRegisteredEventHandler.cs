using eShop.Distribution.Exceptions;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class AccountRegisteredEventHandler : IConsumer<AccountRegisteredEvent>
    {
        private readonly IAccountService _accountService;

        public AccountRegisteredEventHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<AccountRegisteredEvent> context)
        {
            var @event = context.Message;
            try
            {
                var account = @event.Account;
                await _accountService.CreateAccountAsync(account.Id, account.FirstName, account.LastName, @event.ProviderId);
            }
            catch (AccountAlreadyExistsException)
            {
            }
        }
    }
}
