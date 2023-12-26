using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync();
        Task<Currency?> GetCurrencyAsync(Guid currencyId);
        Task SyncCurrenciesAsync(IEnumerable<Currency> currencies);
    }
}
