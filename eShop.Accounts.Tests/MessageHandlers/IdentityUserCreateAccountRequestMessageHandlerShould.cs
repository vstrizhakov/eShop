using eShop.Accounts.Entities;
using eShop.Accounts.MessageHandlers;
using eShop.Accounts.Repositories;
using eShop.Messaging.Models.Identity;

namespace eShop.Accounts.Tests.MessageHandlers
{
    public class IdentityUserCreateAccountRequestMessageHandlerShould
    {
        [Fact]
        public async Task CreateAccount()
        {
            // Arrange

            Account? resultAccount = null;

            var result = new RegisterIdentityUserRequest
            {
                IdentityUserId = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                PhoneNumber = "+380000000000",
            };

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.CreateAccountAsync(It.IsAny<Account>()))
                .Callback<Account>(account =>
                {
                    resultAccount = account;
                })
                .Returns(Task.CompletedTask);

            var sut = new RegisterIdentityUserRequestHandler(accountRepository.Object);

            // Act

            await sut.HandleRequestAsync(result);

            // Assert

            accountRepository.VerifyAll();

            Assert.NotNull(resultAccount);
            Assert.Equal(result.IdentityUserId, resultAccount.IdentityUserId);
            Assert.Equal(result.FirstName, resultAccount.FirstName);
            Assert.Equal(result.LastName, resultAccount.LastName);
            Assert.Equal(result.Email, resultAccount.Email);
            Assert.Equal(result.PhoneNumber, resultAccount.PhoneNumber);
        }

        [Fact]
        public async Task PublishResponse()
        {
            // Arrange

            RegisterIdentityUserResponse? result = null;

            var request = new RegisterIdentityUserRequest
            {
                IdentityUserId = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                PhoneNumber = "+380000000000",
            };

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.CreateAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);

            var sut = new RegisterIdentityUserRequestHandler(accountRepository.Object);

            // Act

            await sut.HandleRequestAsync(request);

            // Assert

            accountRepository.VerifyAll();

            Assert.NotNull(result);
            Assert.Equal(request.IdentityUserId, result.IdentityUserId);
            Assert.NotEqual(default, result.AccountId);
        }
    }
}
