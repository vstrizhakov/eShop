using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class ViberChatUpdatedEventHandler : IMessageHandler<ViberChatUpdatedEvent>
    {
        private readonly IAccountService _accountService;

        public ViberChatUpdatedEventHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task HandleMessageAsync(ViberChatUpdatedEvent message)
        {
            try
            {
                await _accountService.UpdateViberChatAsync(message.AccountId, message.ViberUserId, message.IsEnabled);
            }
            catch (AccountNotFoundException)
            {
            }
        }
    }
}
