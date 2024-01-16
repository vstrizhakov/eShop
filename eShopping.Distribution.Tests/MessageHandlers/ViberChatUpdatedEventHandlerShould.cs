using eShopping.Distribution.Entities;
using eShopping.Distribution.Exceptions;
using eShopping.Distribution.MessageHandlers;
using eShopping.Distribution.Repositories;
using eShopping.Distribution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopping.Distribution.Tests.MessageHandlers
{
    public class ViberChatUpdatedEventHandlerShould
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task UpdateViberChat(bool isEnabled)
        {
            // Arrange

            var message = new Messaging.Contracts.ViberChatUpdatedEvent
            {
                AccountId = Guid.NewGuid(),
                ViberUserId = Guid.NewGuid(),
                IsEnabled = isEnabled,
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.UpdateViberChatAsync(message.AccountId, message.ViberUserId, message.IsEnabled))
                .Returns(Task.CompletedTask);

            var sut = new ViberChatUpdatedEventHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }

        [Fact]
        public async Task HandleAccountNotFoundException_OnUpdateViberChat()
        {
            // Arrange

            var message = new Messaging.Contracts.ViberChatUpdatedEvent
            {
                AccountId = Guid.NewGuid(),
                ViberUserId = Guid.NewGuid(),
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.UpdateViberChatAsync(message.AccountId, message.ViberUserId, message.IsEnabled))
                .ThrowsAsync(new AccountNotFoundException());

            var sut = new ViberChatUpdatedEventHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }
    }
}
