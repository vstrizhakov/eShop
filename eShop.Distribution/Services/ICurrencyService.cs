using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface ICurrencyService
    {
        Task SyncCurrenciesAsync(IEnumerable<Currency> currencies);
    }
}
