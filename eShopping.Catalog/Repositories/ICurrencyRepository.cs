using eShopping.Catalog.Entities;

namespace eShopping.Catalog.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync();
        Task<Currency?> GetCurrencyByIdAsync(Guid id);
        Task CreateCurrencyAsync(Currency currency);
        Task DeleteCurrencyAsync(Currency currency);
        Task<IEnumerable<Currency>> GetCurrenciesAsync(IEnumerable<Guid> currencyIds);
    }
}
