using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class ViberUserCreateAccountUpdateMessageHandler : IMessageHandler<ViberUserCreateAccountUpdateMessage>
    {
        private readonly IAccountRepository _repository;

        public ViberUserCreateAccountUpdateMessageHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleMessageAsync(ViberUserCreateAccountUpdateMessage message)
        {
            if (message.IsSuccess)
            {
                var accountId = message.AccountId.Value;
                var account = await _repository.GetAccountByIdAsync(accountId);
                if (account == null)
                {
                    account = new Account
                    {
                        Id = accountId,
                        ProviderId = message.ProviderId.Value,
                    };

                    await _repository.CreateAccountAsync(account);
                }
            }
        }
    }
}
