using eShop.Distribution.Entities;
using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;
using eShop.Distribution.Services;

namespace eShop.Distribution.Tests.Services
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
                .Setup(e => e.GetAccountsByProviderIdAsync(providerId, true, true))
                .ReturnsAsync(accounts);

            DistributionGroup? result = null;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.CreateDistributionGroupAsync(It.IsAny<DistributionGroup>()))
                .Callback<DistributionGroup>(distribution => result = distribution);

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act

            await sut.CreateDistributionFromProviderIdAsync(providerId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(providerId, result.ProviderId);
            Assert.Contains(result.Items, item => item.ViberChatId == accounts[0].ViberChat.Id);
            Assert.Contains(result.Items, item => item.TelegramChatId == accounts[0].TelegramChats.ElementAt(0).Id);
            Assert.DoesNotContain(result.Items, item => item.ViberChatId == accounts[1].ViberChat.Id);
            Assert.DoesNotContain(result.Items, item => item.TelegramChatId == accounts[1].TelegramChats.ElementAt(0).Id);
            Assert.DoesNotContain(result.Items, item => !item.ViberChatId.HasValue && !item.TelegramChatId.HasValue);
            Assert.All(result.Items, item => Assert.Equal(DistributionGroupItemStatus.Pending, item.Status));
        }

        [Fact]
        public async Task SetDistributionRequestStatusToDelivered()
        {
            // Arrange

            var distributionRequest = new DistributionGroupItem();
            var distributionRequestId = distributionRequest.Id;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionRequestAsync(distributionRequestId))
                .ReturnsAsync(distributionRequest);
            distributionRepository
                .Setup(e => e.UpdateDistributionGroupItemAsync(distributionRequest))
                .Returns(Task.CompletedTask);

            var accountRepository = new Mock<IAccountRepository>();

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act

            await sut.UpdateDistributionRequestStatusAsync(distributionRequestId, false);

            // Assert

            distributionRepository.VerifyAll();
            accountRepository.VerifyAll();

            Assert.Equal(DistributionGroupItemStatus.Delivered, distributionRequest.Status);
        }

        [Fact]
        public async Task SetDistributionRequestStatusToFailed()
        {
            // Arrange

            var distributionRequest = new DistributionGroupItem();
            var distributionRequestId = distributionRequest.Id;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionRequestAsync(distributionRequestId))
                .ReturnsAsync(distributionRequest);
            distributionRepository
                .Setup(e => e.UpdateDistributionGroupItemAsync(distributionRequest))
                .Returns(Task.CompletedTask);

            var accountRepository = new Mock<IAccountRepository>();

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act

            await sut.UpdateDistributionRequestStatusAsync(distributionRequestId, true);

            // Assert

            distributionRepository.VerifyAll();
            accountRepository.VerifyAll();

            Assert.Equal(DistributionGroupItemStatus.Failed, distributionRequest.Status);
        }

        [Fact]
        public async Task ThrowInvalidDistributionRequestStatusException()
        {
            // Arrange

            var distributionRequest = new DistributionGroupItem
            {
                Status = DistributionGroupItemStatus.Delivered,
            };
            var distributionRequestId = distributionRequest.Id;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionRequestAsync(distributionRequestId))
                .ReturnsAsync(distributionRequest);
            distributionRepository
                .Setup(e => e.UpdateDistributionGroupItemAsync(distributionRequest))
                .Returns(Task.CompletedTask);

            var accountRepository = new Mock<IAccountRepository>();

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act & Assert

            await Assert.ThrowsAsync<InvalidDistributionRequestStatusException>(async () =>
            {
                await sut.UpdateDistributionRequestStatusAsync(distributionRequestId, true);
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
                .ReturnsAsync(default(DistributionGroupItem));

            var accountRepository = new Mock<IAccountRepository>();

            var sut = new DistributionService(distributionRepository.Object, accountRepository.Object);

            // Act & Assert

            await Assert.ThrowsAsync<DistributionRequestNotFoundException>(async () =>
            {
                await sut.UpdateDistributionRequestStatusAsync(distributionRequestId, true);
            });
        }
    }
}
