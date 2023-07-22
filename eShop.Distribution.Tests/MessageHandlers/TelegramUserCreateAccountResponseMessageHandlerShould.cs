using eShop.Distribution.Exceptions;
using eShop.Distribution.MessageHandlers;
using eShop.Distribution.Services;
using eShop.Messaging.Models;

namespace eShop.Distribution.Tests.MessageHandlers
{
    public class TelegramUserCreateAccountResponseMessageHandlerShould
    {
        [Fact]
        public async Task CreateNewAccount()
        {
            // Arrange

            var message = new TelegramUserCreateAccountResponseMessage
            {
                AccountId = Guid.NewGuid(),
                ProviderId = Guid.NewGuid(),
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.CreateNewAccountAsync(message.AccountId, message.ProviderId))
                .Returns(Task.CompletedTask);

            var sut = new TelegramUserCreateAccountResponseMessageHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }

        [Fact]
        public async Task HandleAccountAlreadyExistsException()
        {
            // Arrange

            var message = new TelegramUserCreateAccountResponseMessage
            {
                AccountId = Guid.NewGuid(),
                ProviderId = Guid.NewGuid(),
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.CreateNewAccountAsync(message.AccountId, message.ProviderId))
                .ThrowsAsync(new AccountAlreadyExistsException());

            var sut = new TelegramUserCreateAccountResponseMessageHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }
    }
}
