using eShop.Distribution.Entities;
using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;
using eShop.Distribution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Distribution.Tests.Services
{
    public class AccountServiceShould
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateTelegramChat(bool isEnabled)
        {
            // Arrange

            var account = new Account();

            var accountId = account.Id;
            var telegramChatId = Guid.NewGuid();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(accountId, null))
                .ReturnsAsync(account);
            accountRepository
                .Setup(e => e.UpdateTelegramChatAsync(account, telegramChatId, isEnabled))
                .Returns(Task.CompletedTask);

            var sut = new AccountService(accountRepository.Object);

            // Act

            await sut.UpdateTelegramChatAsync(accountId, telegramChatId, isEnabled);

            // Assert

            accountRepository.VerifyAll();
        }

        [Fact]
        public async Task ThrowAccountNotFoundException_OnUpdateTelegramChat()
        {
            // Arrange

            var accountId = Guid.NewGuid();
            var telegramChatId = Guid.NewGuid();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(accountId, null))
                .ReturnsAsync(default(Account));

            var sut = new AccountService(accountRepository.Object);

            // Act & Assert

            await Assert.ThrowsAsync<AccountNotFoundException>(async () =>
            {
                await sut.UpdateTelegramChatAsync(accountId, telegramChatId, false);
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateViberChat(bool isEnabled)
        {
            // Arrange

            var account = new Account();

            var accountId = account.Id;
            var viberChatId = Guid.NewGuid();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(accountId, null))
                .ReturnsAsync(account);
            accountRepository
                .Setup(e => e.UpdateViberChatAsync(account, viberChatId, isEnabled))
                .Returns(Task.CompletedTask);

            var sut = new AccountService(accountRepository.Object);

            // Act

            await sut.UpdateViberChatAsync(accountId, viberChatId, isEnabled);

            // Assert

            accountRepository.VerifyAll();
        }

        [Fact]
        public async Task ThrowAccountNotFoundException_OnUpdateViberChat()
        {
            // Arrange

            var accountId = Guid.NewGuid();
            var viberChatId = Guid.NewGuid();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(accountId, null))
                .ReturnsAsync(default(Account));

            var sut = new AccountService(accountRepository.Object);

            // Act & Assert

            await Assert.ThrowsAsync<AccountNotFoundException>(async () =>
            {
                await sut.UpdateViberChatAsync(accountId, viberChatId, false);
            });
        }

        [Fact]
        public async Task CreateNewAccount()
        {
            // Arrange

            var accountId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Smith";

            Account? result = null;

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(accountId, null))
                .ReturnsAsync(default(Account));
            accountRepository
                .Setup(e => e.CreateAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask)
                .Callback<Account>(account => result = account);

            var sut = new AccountService(accountRepository.Object);

            // Act

            await sut.CreateAccountAsync(accountId, firstName, lastName, providerId);

            // Assert

            accountRepository.VerifyAll();

            Assert.NotNull(result);
            Assert.Equal(providerId, result.AnnouncerId);
            Assert.NotNull(result.ActiveDistributionSettings);
        }

        [Fact]
        public async Task ThrowAccountAlreadyExistsException_OnCreateNewAccount()
        {
            // Arrange

            var account = new Account();

            var accountId = account.Id;
            var providerId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Smith";

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(accountId, null))
                .ReturnsAsync(account);

            var sut = new AccountService(accountRepository.Object);

            // Act & Assert

            await Assert.ThrowsAsync<AccountAlreadyExistsException>(async () =>
            {
                await sut.CreateAccountAsync(accountId, firstName, lastName, providerId);
            });
        }
    }
}
