using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IDistributionSettingsRepository
    {
        Task CreateDistributionSettingsAsync(DistributionSettings distributionSettings);
        Task<DistributionSettings?> GetActiveDistributionSettingsAsync(Guid accountId);
    }
}
