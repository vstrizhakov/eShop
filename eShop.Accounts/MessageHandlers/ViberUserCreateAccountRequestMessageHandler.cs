using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Repositories;
using eShop.Accounts.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Accounts.MessageHandlers
{
    public class ViberUserCreateAccountRequestMessageHandler : IMessageHandler<ViberUserCreateAccountRequestMessage>
    {
        private readonly IAccountService _accountService;
        private readonly IProducer _producer;

        public ViberUserCreateAccountRequestMessageHandler(IAccountService accountService, IProducer producer)
        {
            _accountService = accountService;
            _producer = producer;
        }

        public async Task HandleMessageAsync(ViberUserCreateAccountRequestMessage message)
        {
            try
            {
                var providerId = message.ProviderId;
                var accountInfo = new Account
                {
                    FirstName = message.Name,
                    PhoneNumber = message.PhoneNumber,
                    ViberUserId = message.ViberUserId,
                };

                var account = await _accountService.RegisterAccountByViberUserIdAsync(providerId, accountInfo);

                var responseMessage = new ViberUserCreateAccountUpdateMessage
                {
                    IsSuccess = true,
                    AccountId = account.Id,
                    ProviderId = message.ProviderId,
                    ViberUserId = account.ViberUserId,
                };

                _producer.Publish(responseMessage);
            }
            catch (AccountAlreadyRegisteredException)
            {
                // Publish message with error
            }
            catch (ProviderNotExistsException)
            {
                // Publish message with error
            }
            catch (InvalidProviderException)
            {
                // Publish message with error
            }
        }
    }
}
