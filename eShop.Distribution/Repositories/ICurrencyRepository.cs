using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync(IEnumerable<Guid> ids);
        Task CreateCurrencyAsync(Currency currency);
        Task<Currency?> GetCurrencyAsync(Guid currencyId);
    }
}
