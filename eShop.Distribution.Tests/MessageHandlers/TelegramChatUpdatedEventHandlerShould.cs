using eShop.Distribution.Exceptions;
using eShop.Distribution.MessageHandlers;
using eShop.Distribution.Services;

namespace eShop.Distribution.Tests.MessageHandlers
{
    public class TelegramChatUpdatedEventHandlerShould
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task UpdateTelegramChat(bool isEnabled)
        {
            // Arrange

            var message = new Messaging.Models.TelegramChatUpdatedEvent
            {
                AccountId = Guid.NewGuid(),
                TelegramChatId = Guid.NewGuid(),
                IsEnabled = isEnabled,
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.UpdateTelegramChatAsync(message.AccountId, message.TelegramChatId, message.IsEnabled))
                .Returns(Task.CompletedTask);

            var sut = new TelegramChatUpdatedEventHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }

        [Fact]
        public async Task HandleAccountNotFoundException_OnUpdateTelegramChat()
        {
            // Arrange

            var message = new Messaging.Models.TelegramChatUpdatedEvent
            {
                AccountId = Guid.NewGuid(),
                TelegramChatId = Guid.NewGuid(),
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.UpdateTelegramChatAsync(message.AccountId, message.TelegramChatId, message.IsEnabled))
                .ThrowsAsync(new AccountNotFoundException());

            var sut = new TelegramChatUpdatedEventHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }
    }
}
