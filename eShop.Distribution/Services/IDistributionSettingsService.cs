using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IDistributionSettingsService
    {
        Task<DistributionSettings> GetDistributionSettingsAsync(Guid accountId);
        Task<DistributionSettings> SetPreferredCurrencyAsync(Guid accountId, Guid currencyId);
        Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(DistributionSettings distributionSettings);
        Task<DistributionSettings> SetCurrencyRateAsync(Guid accountId, Guid sourceCurrencyId, double rate);
    }
}
