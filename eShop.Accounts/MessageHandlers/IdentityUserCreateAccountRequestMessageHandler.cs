using eShop.Accounts.Entities;
using eShop.Accounts.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.RabbitMq;

namespace eShop.Accounts.MessageHandlers
{
    public class IdentityUserCreateAccountRequestMessageHandler : IMessageHandler<IdentityUserCreateAccountRequestMessage>
    {
        private readonly IAccountRepository _repository;
        private readonly IProducer _producer;

        public IdentityUserCreateAccountRequestMessageHandler(
            IAccountRepository repository,
            IProducer producer)
        {
            _repository = repository;
            _producer = producer;
        }

        public async Task HandleMessageAsync(IdentityUserCreateAccountRequestMessage message)
        {
            var account = new Account
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                Email = message.Email,
                PhoneNumber = message.PhoneNumber,
                IdentityUserId = message.IdentityUserId,
            };

            await _repository.CreateAccountAsync(account);

            var responseMessage = new IdentityUserCreateAccountResponseMessage
            {
                AccountId = account.Id,
                IdentityUserId = message.IdentityUserId,
            };

            _producer.Publish(responseMessage);
        }
    }
}
