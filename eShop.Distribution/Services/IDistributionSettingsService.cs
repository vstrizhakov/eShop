using eShop.Distribution.Aggregates;
using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IDistributionSettingsService
    {
        Task<DistributionSettings?> GetDistributionSettingsAsync(Guid accountId);
        Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(DistributionSettings distributionSettings);
        Task<IEnumerable<ShopFilter>> GetShopsAsync(DistributionSettings distributionSettings);
        Task<DistributionSettings> SetPreferredCurrencyAsync(DistributionSettings distributionSettings, Guid currencyId);
        Task<DistributionSettings> SetCurrencyRateAsync(DistributionSettings distributionSettings, Guid sourceCurrencyId, double rate);
        Task<DistributionSettings> SetComissionAmountAsync(DistributionSettings distributionSettings, decimal amount);
        Task<DistributionSettings> SetFilterShopsAsync(DistributionSettings distributionSettings, bool filter);
        Task<DistributionSettings> SetShopIsEnabledAsync(DistributionSettings distributionSettings, Guid shopId, bool isEnabled);
    }
}
