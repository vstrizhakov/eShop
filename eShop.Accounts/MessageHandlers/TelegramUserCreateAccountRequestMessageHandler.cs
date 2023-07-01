using eShop.Accounts.Entities;
using eShop.Accounts.Repositories;
using eShop.Messaging;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.RabbitMq;

namespace eShop.Accounts.MessageHandlers
{
    public class TelegramUserCreateAccountRequestMessageHandler : IMessageHandler<TelegramUserCreateAccountRequestMessage>
    {
        private readonly IAccountRepository _repository;
        private readonly IRabbitMqProducer _producer;

        public TelegramUserCreateAccountRequestMessageHandler(
            IAccountRepository repository,
            IRabbitMqProducer producer)
        {
            _repository = repository;
            _producer = producer;
        }

        public async Task HandleMessageAsync(TelegramUserCreateAccountRequestMessage message)
        {
            var telegramUserId = message.TelegramUserId;
            var account = await _repository.GetAccountByTelegramUserIdAsync(telegramUserId);
            if (account == null)
            {
                account = new Account
                {
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    TelegramUserId = telegramUserId, 
                };

                await _repository.CreateAccountAsync(account);

                var responseMessage = new TelegramUserCreateAccountResponseMessage
                {
                    AccountId = account.Id,
                    TelegramUserId = telegramUserId,
                    ProviderId = message.ProviderId,
                };

                _producer.Publish(responseMessage);
            }
            else
            {
                // TODO:
            }
        }
    }
}
