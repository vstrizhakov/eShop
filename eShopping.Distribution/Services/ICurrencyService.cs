using eShopping.Distribution.Entities;

namespace eShopping.Distribution.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync();
        Task<Currency?> GetCurrencyAsync(Guid currencyId);
        Task SyncCurrenciesAsync(IEnumerable<Currency> currencies);
    }
}
