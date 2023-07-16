using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Repositories;
using eShop.Accounts.Services;
using Moq;

namespace eShop.Accounts.Tests.Services
{
    public class AccountServiceShould
    {
        [Fact]
        public async Task ThrowArgumentException()
        {
            // Arrange

            var accountRepository = new Mock<IAccountRepository>();

            // Act

            var accountService = new AccountService(accountRepository.Object);

            var providerId = Guid.NewGuid();
            var accountInfo = new Account
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
            };

            // Assert

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await accountService.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);
            });
        }

        [Fact]
        public async Task ThrowAccountAlreadyRegisteredException()
        {
            // Arrange

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByTelegramUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Account());

            // Act

            var accountService = new AccountService(accountRepository.Object);

            var providerId = Guid.NewGuid();
            var accountInfo = new Account
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
                TelegramUserId = Guid.NewGuid(),
            };

            // Assert

            await Assert.ThrowsAsync<AccountAlreadyRegisteredException>(async () =>
            {
                await accountService.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);
            });
        }

        [Fact]
        public async Task ThrowProviderNotExistsException()
        {
            // Arrange

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByTelegramUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act

            var accountService = new AccountService(accountRepository.Object);

            var providerId = Guid.NewGuid();
            var accountInfo = new Account
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
                TelegramUserId = Guid.NewGuid(),
            };

            // Assert

            await Assert.ThrowsAsync<ProviderNotExistsException>(async () =>
            {
                await accountService.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);
            });
        }

        [Fact]
        public async Task ThrowInvalidProviderException()
        {
            // Arrange

            var providerId = Guid.NewGuid();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByTelegramUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Account());
            accountRepository
                .Setup(e => e.GetAccountByPhoneNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new Account
                {
                    Id = providerId,
                });

            // Act

            var accountService = new AccountService(accountRepository.Object);

            var accountInfo = new Account
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
                TelegramUserId = Guid.NewGuid(),
            };

            // Assert

            await Assert.ThrowsAsync<InvalidProviderException>(async () =>
            {
                await accountService.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);
            });
        }

        [Fact]
        public async Task CreateNewAccountByTelegramUserId()
        {
            // Arrange

            Account? result = null;

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByTelegramUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Account());
            accountRepository
                .Setup(e => e.GetAccountByPhoneNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);
            accountRepository
                .Setup(e => e.CreateAccountAsync(It.IsAny<Account>()))
                .Callback<Account>(account =>
                {
                    result = account;
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act

            var accountService = new AccountService(accountRepository.Object);

            var providerId = Guid.NewGuid();
            var accountInfo = new Account
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
                TelegramUserId = Guid.NewGuid(),
            };
            await accountService.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);

            // Assert

            accountRepository.VerifyAll();

            Assert.NotNull(result);
            Assert.Equal(accountInfo.FirstName, result.FirstName);
            Assert.Equal(accountInfo.LastName, result.LastName);
            Assert.Equal(accountInfo.PhoneNumber, result.PhoneNumber);
            Assert.Equal(accountInfo.TelegramUserId, result.TelegramUserId);
        }

        [Fact]
        public async Task LinkTelegramUserToAlreadyExistingAccount()
        {
            // Arrange

            Account? result = null;

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByTelegramUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Account());
            accountRepository
                .Setup(e => e.GetAccountByPhoneNumberAsync(It.IsAny<string>()))
                .ReturnsAsync((string phoneNumber) => new Account
                {
                    PhoneNumber = phoneNumber,
                });

            accountRepository
                .Setup(e => e.UpdateAccountAsync(It.IsAny<Account>()))
                .Callback<Account>(account =>
                {
                    result = account;
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act

            var accountService = new AccountService(accountRepository.Object);

            var providerId = Guid.NewGuid();
            var accountInfo = new Account
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
                TelegramUserId = Guid.NewGuid(),
            };
            await accountService.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);

            // Assert

            accountRepository.VerifyAll();

            Assert.NotNull(result);
            Assert.Equal(accountInfo.TelegramUserId, result.TelegramUserId);
        }
    }
}
