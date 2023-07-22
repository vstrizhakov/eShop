using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Accounts.MessageHandlers
{
    public class TelegramUserCreateAccountRequestMessageHandler : IMessageHandler<TelegramUserCreateAccountRequestMessage>
    {
        private readonly IAccountService _accountService;
        private readonly IProducer _producer;

        public TelegramUserCreateAccountRequestMessageHandler(
            IAccountService accountService,
            IProducer producer)
        {
            _accountService = accountService;
            _producer = producer;
        }

        public async Task HandleMessageAsync(TelegramUserCreateAccountRequestMessage message)
        {
            try
            {
                var providerId = message.ProviderId;
                var accountInfo = new Account
                {
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    PhoneNumber = message.PhoneNumber,
                    TelegramUserId = message.TelegramUserId,
                };
                var account = await _accountService.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);
             
                var responseMessage = new TelegramUserCreateAccountResponseMessage
                {
                    AccountId = account.Id,
                    TelegramUserId = account.TelegramUserId.Value,
                    ProviderId = message.ProviderId,
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
