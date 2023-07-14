using eShop.Accounts.MessageHandlers;
using eShop.Accounts.Repositories;
using eShop.RabbitMq;
using Moq;

namespace eShop.Accounts.Tests
{
    public class TelegramUserCreateAccountRequestMessageHandlerShould
    {
        [Fact]
        public void Test1()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock
                .Setup(e => e.CreateAccountAsync(null))
                .Returns(Task.CompletedTask);

            var producerMock = new Mock<IProducer>();

            var messageHandler = new TelegramUserCreateAccountRequestMessageHandler(
                accountRepositoryMock.Object,
                producerMock.Object);
        }
    }
}