using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IDefaultCurrencyRateRepository
    {
        Task<IEnumerable<DefaultCurrencyRate>> GetCurrencyRatesAsync(Guid targetCurrencyId);
    }
}
