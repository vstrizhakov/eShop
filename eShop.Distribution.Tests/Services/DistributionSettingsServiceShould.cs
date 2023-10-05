using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;
using eShop.Distribution.Services;

namespace eShop.Distribution.Tests.Services
{
    public class DistributionSettingsServiceShould
    {
        [Fact]
        public async Task GetDistributionSettings_WhenAlreadyExists()
        {
            // Arrange

            var accountId = Guid.NewGuid();

            var distributionSettings = new DistributionSettings();

            var distributionSettingsRepository = new Mock<IDistributionSettingsRepository>();
            distributionSettingsRepository
                .Setup(e => e.GetDistributionSettingsAsync(accountId))
                .ReturnsAsync(distributionSettings);

            var sut = new DistributionSettingsService(distributionSettingsRepository.Object);

            // Act

            var result = await sut.GetDistributionSettingsAsync(accountId);

            // Assert

            distributionSettingsRepository.VerifyAll();

            Assert.Equal(distributionSettings, result);
        }

        [Fact]
        public async Task GetDistributionSettings_WhenNotExists()
        {
            // Arrange

            var accountId = Guid.NewGuid();

            var distributionSettingsRepository = new Mock<IDistributionSettingsRepository>();
            distributionSettingsRepository
                .Setup(e => e.GetDistributionSettingsAsync(accountId))
                .ReturnsAsync(default(DistributionSettings));

            var sut = new DistributionSettingsService(distributionSettingsRepository.Object);

            // Act

            var result = await sut.GetDistributionSettingsAsync(accountId);

            // Assert

            distributionSettingsRepository.VerifyAll();

            Assert.NotNull(result);
        }
    }
}
