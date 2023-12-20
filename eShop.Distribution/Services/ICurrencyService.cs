using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface ICurrencyService
    {
        Task<Currency?> GetCurrencyAsync(Guid currencyId);
        Task SyncCurrenciesAsync(IEnumerable<Currency> currencies);
    }
}
