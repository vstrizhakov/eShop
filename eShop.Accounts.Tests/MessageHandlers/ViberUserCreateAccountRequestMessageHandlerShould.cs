using eShop.Accounts.Entities;
using eShop.Accounts.MessageHandlers;
using eShop.Accounts.Services;
using eShop.Messaging.Models;
using eShop.RabbitMq;

namespace eShop.Accounts.Tests.MessageHandlers
{
    public class ViberUserCreateAccountRequestMessageHandlerShould
    {
        [Fact]
        public async Task PublishResponse()
        {
            // Arrange

            ViberUserCreateAccountUpdateMessage? result = null;

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.RegisterAccountByViberUserIdAsync(It.IsAny<Guid>(), It.IsAny<Account>()))
                .ReturnsAsync((Guid providerId, Account account) => new Account
                {
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    PhoneNumber = account.PhoneNumber,
                    ViberUserId = account.ViberUserId,
                });

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<ViberUserCreateAccountUpdateMessage>()))
                .Callback<ViberUserCreateAccountUpdateMessage>(message => result = message)
                .Verifiable();

            // Act

            var messageHandler = new ViberUserCreateAccountRequestMessageHandler(accountService.Object, producer.Object);

            var message = new ViberUserCreateAccountRequestMessage
            {
                Name = "John",
                PhoneNumber = "+380000000000",
                ProviderId = Guid.NewGuid(),
                ViberUserId = Guid.NewGuid(),
            };
            await messageHandler.HandleMessageAsync(message);

            // Assert

            producer.VerifyAll();

            Assert.NotNull(result);
            Assert.Equal(message.ViberUserId, result.ViberUserId);
            Assert.Equal(message.ProviderId, result.ProviderId);
            Assert.NotEqual(default, result.AccountId);
        }
    }
}
