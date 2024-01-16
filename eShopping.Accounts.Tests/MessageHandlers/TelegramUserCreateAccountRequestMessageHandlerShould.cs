using eShopping.Accounts.Entities;
using eShopping.Accounts.Consumers;
using eShopping.Accounts.Services;
using eShopping.Messaging.Contracts.Telegram;
using eShopping.Messaging.Contracts.Telegram;

namespace eShopping.Accounts.Tests.MessageHandlers
{
    public class TelegramUserCreateAccountRequestMessageHandlerShould
    {
        [Fact]
        public async void PublishResponse()
        {
            // Arrange

            RegisterTelegramUserResponse? result = null;

            var request = new RegisterTelegramUserRequest
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
                TelegramUserId = Guid.NewGuid(),
                AnnouncerId = Guid.NewGuid(),
            };

            var accountRepository = new Mock<IAccountService>();
            accountRepository
                .Setup(e => e.RegisterAccountByTelegramUserIdAsync(It.IsAny<Guid>(), It.IsAny<Account>()))
                .ReturnsAsync((Guid providerId, Account account) => new Account
                {
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    PhoneNumber = account.PhoneNumber,
                    TelegramUserId = account.TelegramUserId,
                });

            var sut = new RegisterTelegramUserRequestHandler(accountRepository.Object);

            // Act

            await sut.HandleRequestAsync(request);

            // Assert

            accountRepository.VerifyAll();

            Assert.NotNull(result);
            Assert.Equal(request.TelegramUserId, result.TelegramUserId);
            Assert.NotEqual(default, result.AccountId);
        }
    }
}