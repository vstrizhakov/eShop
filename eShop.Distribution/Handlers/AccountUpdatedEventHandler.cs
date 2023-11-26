using eShop.Distribution.Exceptions;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.Handlers
{
    public class AccountUpdatedEventHandler : IMessageHandler<AccountUpdatedEvent>
    {
        private readonly IAccountService _accountService;

        public AccountUpdatedEventHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task HandleMessageAsync(AccountUpdatedEvent @event)
        {
            try
            {
                var account = @event.Account;
                await _accountService.UpdateAccountAsync(account.Id, account.FirstName, account.LastName, @event.AnnouncerId);
            }
            catch (AccountAlreadyExistsException)
            {
            }
        }
    }
}
