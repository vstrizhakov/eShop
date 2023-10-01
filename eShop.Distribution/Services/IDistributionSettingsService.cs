using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IDistributionSettingsService
    {
        Task<DistributionSettings> GetDistributionSettingsAsync(Guid accountId);
        Task<DistributionSettings> UpdateDistributionSettingsAsync(Guid accountId, DistributionSettings distributionSettings);
    }
}
