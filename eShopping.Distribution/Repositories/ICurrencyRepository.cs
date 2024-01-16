using eShopping.Distribution.Entities;

namespace eShopping.Distribution.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync();
        Task<IEnumerable<Currency>> GetCurrenciesAsync(IEnumerable<Guid> ids);
        Task CreateCurrencyAsync(Currency currency);
        Task<Currency?> GetCurrencyAsync(Guid currencyId);
    }
}
