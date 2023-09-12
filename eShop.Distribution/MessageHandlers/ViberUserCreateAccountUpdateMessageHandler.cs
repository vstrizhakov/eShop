using eShop.Distribution.Exceptions;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class ViberUserCreateAccountUpdateMessageHandler : IMessageHandler<ViberUserCreateAccountUpdateMessage>
    {
        private readonly IAccountService _accountService;

        public ViberUserCreateAccountUpdateMessageHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task HandleMessageAsync(ViberUserCreateAccountUpdateMessage message)
        {
            if (message.IsSuccess)
            {
                try
                {
                    await _accountService.CreateNewAccountAsync(message.AccountId.Value, message.FirstName, message.LastName, message.ProviderId.Value);
                }
                catch (AccountAlreadyExistsException)
                {
                }
            }
        }
    }
}
