using eShop.Accounts.Entities;
using eShop.Accounts.Repositories;
using eShop.Messaging;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.RabbitMq;

namespace eShop.Accounts.MessageHandlers
{
    public class ViberUserCreateAccountRequestMessageHandler : IMessageHandler<ViberUserCreateAccountRequestMessage>
    {
        private readonly IAccountRepository _repository;
        private readonly IRabbitMqProducer _producer;

        public ViberUserCreateAccountRequestMessageHandler(IAccountRepository repository, IRabbitMqProducer producer)
        {
            _repository = repository;
            _producer = producer;
        }

        public async Task HandleMessageAsync(ViberUserCreateAccountRequestMessage message)
        {
            var account = await _repository.GetAccountByViberUserIdAsync(message.ViberUserId);
            if (account == null)
            {
                account = new Account
                {
                    FirstName = message.Name,
                    ViberUserId = message.ViberUserId,
                };

                await _repository.CreateAccountAsync(account);

                var responseMessage = new ViberUserCreateAccountUpdateMessage
                {
                    IsSuccess = true,
                    AccountId = account.Id,
                    ProviderId = message.ProviderId,
                    ViberUserId = message.ViberUserId,
                };

                _producer.Publish(responseMessage);
            }
        }
    }
}
