using eShopping.Distribution.Entities;
using eShopping.Distribution.Exceptions;
using eShopping.Distribution.Repositories;
using eShopping.Distribution.Services;

namespace eShopping.Distribution.Tests.Services
{
    public class DistributionServiceShould
    {
        [Fact]
        public async Task CreateCompositionFromProviderId()
        {
            // Arrange

            var providerId = Guid.NewGuid();
            var enabledAccount = new Account
            {
                ViberChat = new ViberChat
                {
                    Id = Guid.NewGuid(),
                    IsEnabled = true,
                },
                TelegramChats = new[]
                    {
                        new TelegramChat
                        {
                            Id = Guid.NewGuid(),
                            IsEnabled = true,
                        },
                    },
            };
            enabledAccount.ViberChat.Account = enabledAccount;

            enabledAccount.TelegramChats.ElementAt(0).Account = enabledAccount;
            var disabledAccount = new Account
            {
                ViberChat = new ViberChat
                {
                    Id = Guid.NewGuid(),
                    IsEnabled = false,
                },
                TelegramChats = new[]
                    {
                        new TelegramChat
                        {
                            Id = Guid.NewGuid(),
                            IsEnabled = false,
                        },
                    },
            };
            var accounts = new[]
            {
                enabledAccount,
                disabledAccount,
            };

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountsByAnnouncerIdAsync(providerId, true, true))
                .ReturnsAsync(accounts);

            Entities.Distribution? result = null;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.CreateDistributionGroupAsync(It.IsAny<Distribution>()))
                .Callback<Distribution>(distribution => result = distribution);

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act

            await sut.CreateDistributionFromProviderIdAsync(providerId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(providerId, result.AnnouncerId);
            Assert.Contains(result.Targets, item => item.ViberChatId == accounts[0].ViberChat.Id);
            Assert.Contains(result.Targets, item => item.TelegramChatId == accounts[0].TelegramChats.ElementAt(0).Id);
            Assert.DoesNotContain(result.Targets, item => item.ViberChatId == accounts[1].ViberChat.Id);
            Assert.DoesNotContain(result.Targets, item => item.TelegramChatId == accounts[1].TelegramChats.ElementAt(0).Id);
            Assert.DoesNotContain(result.Targets, item => !item.ViberChatId.HasValue && !item.TelegramChatId.HasValue);
            Assert.All(result.Targets, item => Assert.Equal(DistributionItemStatus.Pending, item.Status));
        }

        [Fact]
        public async Task SetDistributionRequestStatusToDelivered()
        {
            // Arrange

            var distributionRequest = new DistributionItem();
            var distributionRequestId = distributionRequest.Id;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionRequestAsync(distributionRequestId))
                .ReturnsAsync(distributionRequest);
            distributionRepository
                .Setup(e => e.UpdateDistributionItemAsync(distributionRequest))
                .Returns(Task.CompletedTask);

            var accountRepository = new Mock<IAccountRepository>();

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act

            await sut.SetDistributionItemStatusAsync(distributionRequestId, false);

            // Assert

            distributionRepository.VerifyAll();
            accountRepository.VerifyAll();

            Assert.Equal(DistributionItemStatus.Delivered, distributionRequest.Status);
        }

        [Fact]
        public async Task SetDistributionRequestStatusToFailed()
        {
            // Arrange

            var distributionRequest = new DistributionItem();
            var distributionRequestId = distributionRequest.Id;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionRequestAsync(distributionRequestId))
                .ReturnsAsync(distributionRequest);
            distributionRepository
                .Setup(e => e.UpdateDistributionItemAsync(distributionRequest))
                .Returns(Task.CompletedTask);

            var accountRepository = new Mock<IAccountRepository>();

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act

            await sut.SetDistributionItemStatusAsync(distributionRequestId, true);

            // Assert

            distributionRepository.VerifyAll();
            accountRepository.VerifyAll();

            Assert.Equal(DistributionItemStatus.Failed, distributionRequest.Status);
        }

        [Fact]
        public async Task ThrowInvalidDistributionRequestStatusException()
        {
            // Arrange

            var distributionRequest = new DistributionItem
            {
                Status = DistributionItemStatus.Delivered,
            };
            var distributionRequestId = distributionRequest.Id;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionRequestAsync(distributionRequestId))
                .ReturnsAsync(distributionRequest);
            distributionRepository
                .Setup(e => e.UpdateDistributionItemAsync(distributionRequest))
                .Returns(Task.CompletedTask);

            var accountRepository = new Mock<IAccountRepository>();

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act & Assert

            await Assert.ThrowsAsync<InvalidDistributionRequestStatusException>(async () =>
            {
                await sut.SetDistributionItemStatusAsync(distributionRequestId, true);
            });
        }

        [Fact]
        public async Task ThrowDistributionRequestNotFoundException()
        {
            // Arrange

            var distributionRequestId = Guid.NewGuid();

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionRequestAsync(distributionRequestId))
                .ReturnsAsync(default(DistributionItem));

            var accountRepository = new Mock<IAccountRepository>();

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act & Assert

            await Assert.ThrowsAsync<DistributionRequestNotFoundException>(async () =>
            {
                await sut.SetDistributionItemStatusAsync(distributionRequestId, true);
            });
        }
    }
}
