using AutoMapper;
using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Repositories;
using eShop.Accounts.Services;
using eShop.Messaging;

namespace eShop.Accounts.Tests.Services
{
    public class AccountServiceShould
    {
        [Fact]
        public async Task ThrowArgumentException()
        {
            // Arrange

            var accountRepository = new Mock<IAccountRepository>();
            var mapper = new Mock<IMapper>();
            var producer = new Mock<IProducer>();

            // Act

            var accountService = new AccountService(accountRepository.Object, mapper.Object, producer.Object);

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

            var mapper = new Mock<IMapper>();
            var producer = new Mock<IProducer>();

            // Act

            var accountService = new AccountService(accountRepository.Object, mapper.Object, producer.Object);

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

            var mapper = new Mock<IMapper>();
            var producer = new Mock<IProducer>();

            // Act

            var accountService = new AccountService(accountRepository.Object, mapper.Object, producer.Object);

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

            var mapper = new Mock<IMapper>();
            var producer = new Mock<IProducer>();

            // Act

            var accountService = new AccountService(accountRepository.Object, mapper.Object, producer.Object);

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

            Messaging.Models.AccountRegisteredEvent? @event = null;

            var providerId = Guid.NewGuid();
            var telegramUserId = Guid.NewGuid();
            var accountInfo = new Account
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
                TelegramUserId = telegramUserId,
            };

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByTelegramUserIdAsync(telegramUserId))
                .ReturnsAsync(default(Account));
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(providerId))
                .ReturnsAsync(() => new Account());
            accountRepository
                .Setup(e => e.GetAccountByPhoneNumberAsync(accountInfo.PhoneNumber))
                .ReturnsAsync(default(Account));
            accountRepository
                .Setup(e => e.CreateAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);

            var mappedAccount = new Messaging.Models.Account();
            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Messaging.Models.Account>(It.IsAny<Account>()))
                .Returns(mappedAccount);

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<Messaging.Models.AccountRegisteredEvent>()))
                .Callback<Messaging.Models.AccountRegisteredEvent>(message => @event = message);

            var sut = new AccountService(accountRepository.Object, mapper.Object, producer.Object);

            // Act

            var result = await sut.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);

            // Assert

            accountRepository.VerifyAll();
            mapper.VerifyAll();
            producer.VerifyAll();

            Assert.NotNull(result);
            Assert.Equal(accountInfo.FirstName, result.FirstName);
            Assert.Equal(accountInfo.LastName, result.LastName);
            Assert.Equal(accountInfo.PhoneNumber, result.PhoneNumber);
            Assert.Equal(accountInfo.TelegramUserId, result.TelegramUserId);

            Assert.Equal(providerId, @event!.ProviderId);
        }

        [Fact]
        public async Task LinkTelegramUserToAlreadyExistingAccount()
        {
            // Arrange

            var providerId = Guid.NewGuid();
            var telegramUserId = Guid.NewGuid();
            var accountInfo = new Account
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+380000000000",
                TelegramUserId = telegramUserId,
            };

            var viberUserId = Guid.NewGuid();
            var identityUserId = Guid.NewGuid().ToString();
            var existingAccount = new Account
            {
                ViberUserId = viberUserId,
                IdentityUserId = identityUserId,
            };

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByTelegramUserIdAsync(telegramUserId))
                .ReturnsAsync(default(Account));
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(providerId))
                .ReturnsAsync(() => new Account());
            accountRepository
                .Setup(e => e.GetAccountByPhoneNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(existingAccount);
            accountRepository
                .Setup(e => e.UpdateAccountAsync(existingAccount))
                .Returns(Task.CompletedTask);

            var mappedAccount = new Messaging.Models.Account();
            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Messaging.Models.Account>(existingAccount))
                .Returns(mappedAccount);

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<Messaging.Models.AccountUpdatedEvent>()));

            var sut = new AccountService(accountRepository.Object, mapper.Object, producer.Object);

            // Act

            var result = await sut.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);

            // Assert

            accountRepository.VerifyAll();
            mapper.VerifyAll();
            producer.VerifyAll();

            Assert.NotNull(result);
            Assert.Equal(telegramUserId, result.TelegramUserId);
            Assert.Equal(viberUserId, result.ViberUserId);
            Assert.Equal(identityUserId, result.IdentityUserId);
        }
    }
}
