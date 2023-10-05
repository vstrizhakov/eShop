using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IDistributionSettingsRepository
    {
        Task UpdateDistributionSettingsAsync(DistributionSettings distributionSettings);
        Task<DistributionSettings?> GetDistributionSettingsAsync(Guid accountId);
        Task<IEnumerable<CurrencyRate>> GetDefaultCurrencyRatesAsync(Guid targetCurrencyId);
    }
}
