using eShop.Distribution.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class TelegramChatUpdatedEventHandler : IMessageHandler<TelegramChatUpdatedEvent>
    {
        private readonly IAccountRepository _repository;

        public TelegramChatUpdatedEventHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleMessageAsync(TelegramChatUpdatedEvent message)
        {
            var account = await _repository.GetAccountByIdAsync(message.AccountId);
            if (account != null)
            {
                await _repository.UpdateTelegramChatAsync(account, message.TelegramChatId, message.IsEnabled);
            }
        }
    }
}
