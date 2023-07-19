using eShop.Accounts.Entities;
using eShop.Accounts.MessageHandlers;
using eShop.Accounts.Repositories;
using eShop.Messaging.Models;
using eShop.RabbitMq;

namespace eShop.Accounts.Tests.MessageHandlers
{
    public class IdentityUserCreateAccountRequestMessageHandlerShould
    {
        [Fact]
        public async Task CreateAccount()
        {
            // Arrange

            Account? resultAccount = null;

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.CreateAccountAsync(It.IsAny<Account>()))
                .Callback<Account>(account =>
                {
                    resultAccount = account;
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish<IdentityUserCreateAccountResponseMessage>(null));

            // Act

            var messageHandler = new IdentityUserCreateAccountRequestMessageHandler(accountRepository.Object, producer.Object);

            var message = new IdentityUserCreateAccountRequestMessage
            {
                IdentityUserId = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                PhoneNumber = "+380000000000",
            };
            await messageHandler.HandleMessageAsync(message);

            // Assert

            accountRepository.Verify();

            Assert.NotNull(resultAccount);
            Assert.Equal(message.IdentityUserId, resultAccount.IdentityUserId);
            Assert.Equal(message.FirstName, resultAccount.FirstName);
            Assert.Equal(message.LastName, resultAccount.LastName);
            Assert.Equal(message.Email, resultAccount.Email);
            Assert.Equal(message.PhoneNumber, resultAccount.PhoneNumber);
        }

        [Fact]
        public async Task PublishResponse()
        {
            // Arrange

            IdentityUserCreateAccountResponseMessage? result = null;

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.CreateAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<IdentityUserCreateAccountResponseMessage>()))
                .Callback<IdentityUserCreateAccountResponseMessage>(response => result = response)
                .Verifiable();

            // Act

            var messageHandler = new IdentityUserCreateAccountRequestMessageHandler(accountRepository.Object, producer.Object);

            var message = new IdentityUserCreateAccountRequestMessage
            {
                IdentityUserId = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                PhoneNumber = "+380000000000",
            };
            await messageHandler.HandleMessageAsync(message);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(message.IdentityUserId, result.IdentityUserId);
            Assert.NotEqual(default, result.AccountId);
        }
    }
}
