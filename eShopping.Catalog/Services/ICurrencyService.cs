using eShopping.Catalog.Entities;

namespace eShopping.Catalog.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync(IEnumerable<Guid> currencyIds);
    }
}