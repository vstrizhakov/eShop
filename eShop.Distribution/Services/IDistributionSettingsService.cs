using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IDistributionSettingsService
    {
        Task<DistributionSettings> GetDistributionSettingsAsync(Guid providerId);
        Task<DistributionSettings> UpdateDistributionSettingsAsync(Guid providerId, DistributionSettings distributionSettings);
    }
}
