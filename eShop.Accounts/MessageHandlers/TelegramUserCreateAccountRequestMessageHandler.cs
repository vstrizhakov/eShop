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
        private readonly IProducer _producer;

        public TelegramUserCreateAccountRequestMessageHandler(
            IAccountRepository repository,
            IProducer producer)
        {
            _repository = repository;
            _producer = producer;
        }

        public async Task HandleMessageAsync(TelegramUserCreateAccountRequestMessage message)
        {
            var providerId = message.ProviderId;
            var provider = await _repository.GetAccountByIdAsync(providerId);
            if (provider != null)
            {
                var telegramUserId = message.TelegramUserId;
                var account = await _repository.GetAccountByTelegramUserIdAsync(telegramUserId);
                if (account == null)
                {
                    var phoneNumber = message.PhoneNumber;
                    account = await _repository.GetAccountByPhoneNumberAsync(phoneNumber);
                    if (account == null)
                    {
                        account = new Account
                        {
                            FirstName = message.FirstName,
                            LastName = message.LastName,
                            PhoneNumber = phoneNumber,
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
                        if (account.Id != providerId)
                        {
                            account.TelegramUserId = telegramUserId;

                            await _repository.UpdateAccountAsync(account);

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
                            // TODO: Handle
                        }
                    }
                }
                else
                {
                    // TODO:
                }
            }
            else
            {
                // TODO:: Handle provider is wrong
            }
        }
    }
}
