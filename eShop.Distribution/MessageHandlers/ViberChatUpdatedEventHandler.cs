using eShop.Distribution.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class ViberChatUpdatedEventHandler : IMessageHandler<ViberChatUpdatedEvent>
    {
        private readonly IAccountRepository _repository;

        public ViberChatUpdatedEventHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleMessageAsync(ViberChatUpdatedEvent message)
        {
            var account = await _repository.GetAccountByIdAsync(message.AccountId);
            if (account != null)
            {
                await _repository.UpdateViberChatAsync(account, message.ViberUserId, message.IsEnabled);
            }
        }
    }
}
