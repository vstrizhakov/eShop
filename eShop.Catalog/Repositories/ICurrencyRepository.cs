using eShop.Catalog.Entities;

namespace eShop.Catalog.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync();
        Task<Currency?> GetCurrencyByIdAsync(Guid id);
        Task CreateCurrencyAsync(Currency currency);
        Task DeleteCurrencyAsync(Currency currency);
    }
}
