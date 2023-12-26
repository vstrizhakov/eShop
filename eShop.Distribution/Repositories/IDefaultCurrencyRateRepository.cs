using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IDefaultCurrencyRateRepository
    {
        Task<IEnumerable<DefaultCurrencyRate>> GetCurrencyRatesAsync(Guid targetCurrencyId);
        Task AddCurrencyRateAsync(DefaultCurrencyRate defaultCurrencyRate);
        Task RemoveCurrencyRateAsync(DefaultCurrencyRate defaultCurrencyRate);
        Task UpdateCurrencyRateAsync(DefaultCurrencyRate defaultCurrencyRate);
        Task<IEnumerable<DefaultCurrencyRate>> GetCurrencyRatesAsync();
    }
}
