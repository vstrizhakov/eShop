using eShop.Distribution.Exceptions;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class TelegramUserCreateAccountResponseMessageHandler : IMessageHandler<TelegramUserCreateAccountResponseMessage>
    {
        private readonly IAccountService _accountService;

        public TelegramUserCreateAccountResponseMessageHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        public async Task HandleMessageAsync(TelegramUserCreateAccountResponseMessage message)
        {
            try
            {
                await _accountService.CreateNewAccountAsync(message.AccountId, message.FirstName, message.LastName, message.ProviderId);
            }
            catch (AccountAlreadyExistsException)
            {
            }
        }
    }
}
