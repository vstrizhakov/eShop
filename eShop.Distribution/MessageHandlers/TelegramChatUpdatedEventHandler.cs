using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class TelegramChatUpdatedEventHandler : IMessageHandler<TelegramChatUpdatedEvent>
    {
        private readonly IAccountService _accountService;

        public TelegramChatUpdatedEventHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task HandleMessageAsync(TelegramChatUpdatedEvent message)
        {
            try
            {
                await _accountService.UpdateTelegramChatAsync(message.AccountId, message.TelegramChatId, message.IsEnabled);
            }
            catch (AccountNotFoundException)
            {
            }
        }
    }
}
