using eShop.Accounts.Entities;
using eShop.Accounts.MessageHandlers;
using eShop.Accounts.Services;
using eShop.Messaging.Models;
using eShop.RabbitMq;
using Moq;

namespace eShop.Accounts.Tests.MessageHandlers
{
    public class TelegramUserCreateAccountRequestMessageHandlerShould
    {
        [Fact]
        public async void PublishResponse()
        {
            // Arrange

            TelegramUserCreateAccountResponseMessage? result = null;

            var accountRepository = new Mock<IAccountService>();
            accountRepository
                .Setup(e => e.RegisterAccountByTelegramUserIdAsync(It.IsAny<Guid>(), It.IsAny<Account>()))
                .ReturnsAsync((Guid providerId, Account account) => new Account
                {
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    PhoneNumber = account.PhoneNumber,
                    TelegramUserId = account.TelegramUserId,
                });

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<TelegramUserCreateAccountResponseMessage>()))
                .Callback<TelegramUserCreateAccountResponseMessage>(message => result = message)
                .Verifiable();

            // Act

            var messageHandler = new TelegramUserCreateAccountRequestMessageHandler(
                accountRepository.Object,
                producer.Object);

            var message = new TelegramUserCreateAccountRequestMessage
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
                TelegramUserId = Guid.NewGuid(),
                ProviderId = Guid.NewGuid(),
            };
            await messageHandler.HandleMessageAsync(message);

            // Assert

            producer.VerifyAll();
            Assert.NotNull(result);
            Assert.Equal(message.ProviderId, result.ProviderId);
            Assert.Equal(message.TelegramUserId, result.TelegramUserId);
            Assert.NotEqual(default, result.AccountId);
        }
    }
}