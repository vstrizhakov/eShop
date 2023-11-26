using eShop.Accounts.Entities;
using eShop.Accounts.MessageHandlers;
using eShop.Accounts.Services;
using eShop.Messaging.Models.Viber;

namespace eShop.Accounts.Tests.MessageHandlers
{
    public class ViberUserCreateAccountRequestMessageHandlerShould
    {
        [Fact]
        public async Task PublishResponse()
        {
            // Arrange

            RegisterViberUserResponse? result = null;

            var request = new RegisterViberUserRequest
            {
                Name = "John",
                PhoneNumber = "+380000000000",
                AnnouncerId = Guid.NewGuid(),
                ViberUserId = Guid.NewGuid(),
            };

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

            var sut = new RegisterViberUserRequestHandler(accountService.Object);

            // Act

            await sut.HandleRequestAsync(request);

            // Assert

            accountService.VerifyAll();

            Assert.NotNull(result);
            Assert.Equal(request.ViberUserId, result.ViberUserId);
            Assert.NotEqual(default, result.AccountId);
        }
    }
}
