using eShop.Distribution.Exceptions;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class AccountRegisteredEventHandler : IMessageHandler<AccountRegisteredEvent>
    {
        private readonly IAccountService _accountService;

        public AccountRegisteredEventHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        public async Task HandleMessageAsync(AccountRegisteredEvent @event)
        {
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
