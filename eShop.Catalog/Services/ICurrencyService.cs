using eShop.Catalog.Entities;

namespace eShop.Catalog.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync(IEnumerable<Guid> currencyIds);
    }
}