﻿using eShop.Distribution.Exceptions;
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

            var message = new RegisterTelegramUserResponse
            {
                AccountId = Guid.NewGuid(),
                ProviderId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Smith",
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.CreateNewAccountAsync(message.AccountId, message.FirstName, message.LastName, message.ProviderId))
                .Returns(Task.CompletedTask);

            var sut = new AccountRegisteredEventHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }

        [Fact]
        public async Task HandleAccountAlreadyExistsException()
        {
            // Arrange

            var message = new RegisterTelegramUserResponse
            {
                AccountId = Guid.NewGuid(),
                ProviderId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Smith",
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.CreateNewAccountAsync(message.AccountId, message.FirstName, message.LastName, message.ProviderId))
                .ThrowsAsync(new AccountAlreadyExistsException());

            var sut = new AccountRegisteredEventHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }
    }
}
