using eShop.Distribution.Exceptions;
using eShop.Distribution.MessageHandlers;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;

namespace eShop.Distribution.Tests.MessageHandlers
{
    public class ViberUserCreateAccountUpdateMessageHandlerShould
    {
        [Fact]
        public async Task CreateNewAccount()
        {
            // Arrange

            var message = new RegisterViberUserResponse
            {
                IsSuccess = true,
                AccountId = Guid.NewGuid(),
                ProviderId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Smith",
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.CreateNewAccountAsync(message.AccountId.Value, message.FirstName, message.LastName, message.ProviderId.Value))
                .Returns(Task.CompletedTask);

            var sut = new AccountUpdatedEventHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }

        [Fact]
        public async Task HandleAccountAlreadyExistsException()
        {
            // Arrange

            var message = new RegisterViberUserResponse
            {
                IsSuccess = true,
                AccountId = Guid.NewGuid(),
                ProviderId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Smith",
            };

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(e => e.CreateNewAccountAsync(message.AccountId.Value, message.FirstName, message.LastName, message.ProviderId.Value))
                .ThrowsAsync(new AccountAlreadyExistsException());

            var sut = new AccountUpdatedEventHandler(accountService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            accountService.VerifyAll();
        }
    }
}
