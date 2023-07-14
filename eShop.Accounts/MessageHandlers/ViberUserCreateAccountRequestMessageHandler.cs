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
        private readonly IProducer _producer;

        public ViberUserCreateAccountRequestMessageHandler(IAccountRepository repository, IProducer producer)
        {
            _repository = repository;
            _producer = producer;
        }

        public async Task HandleMessageAsync(ViberUserCreateAccountRequestMessage message)
        {
            var providerId = message.ProviderId;
            var provider = await _repository.GetAccountByIdAsync(providerId);
            if (provider != null)
            {
                var viberUserId = message.ViberUserId;
                var account = await _repository.GetAccountByViberUserIdAsync(viberUserId);
                if (account == null)
                {
                    var phoneNumber = message.PhoneNumber;
                    account = await _repository.GetAccountByPhoneNumberAsync(phoneNumber);
                    if (account == null)
                    {
                        account = new Account
                        {
                            FirstName = message.Name,
                            PhoneNumber = phoneNumber,
                            ViberUserId = viberUserId,
                        };

                        await _repository.CreateAccountAsync(account);

                        var responseMessage = new ViberUserCreateAccountUpdateMessage
                        {
                            IsSuccess = true,
                            AccountId = account.Id,
                            ProviderId = message.ProviderId,
                            ViberUserId = viberUserId,
                        };

                        _producer.Publish(responseMessage);
                    }
                    else
                    {
                        if (account.Id != providerId)
                        {
                            account.ViberUserId = viberUserId;

                            await _repository.UpdateAccountAsync(account);

                            var responseMessage = new ViberUserCreateAccountUpdateMessage
                            {
                                IsSuccess = true,
                                AccountId = account.Id,
                                ProviderId = message.ProviderId,
                                ViberUserId = viberUserId,
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
                // TODO: Handle provider is wrong
            }
        }
    }
}
