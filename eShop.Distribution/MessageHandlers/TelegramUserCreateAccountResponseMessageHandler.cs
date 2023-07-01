using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class TelegramUserCreateAccountResponseMessageHandler : IMessageHandler<TelegramUserCreateAccountResponseMessage>
    {
        private readonly IAccountRepository _repository;

        public TelegramUserCreateAccountResponseMessageHandler(IAccountRepository repository)
        {
            _repository = repository;
        }
        
        public async Task HandleMessageAsync(TelegramUserCreateAccountResponseMessage message)
        {
            var accountId = message.AccountId;
            var account = await _repository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                account = new Account
                {
                    Id = accountId,
                    ProviderId = message.ProviderId,
                };

                await _repository.CreateAccountAsync(account);
            }
        }
    }
}
