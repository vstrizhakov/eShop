using eShopping.Distribution.Entities;

namespace eShopping.Distribution.Services
{
    public interface IDefaultCurrencyRateService
    {
        Task<IEnumerable<DefaultCurrencyRate>> GetAllAsync();
        Task AddAsync(DefaultCurrencyRate defaultCurrencyRates);
        Task RemoveAsync(DefaultCurrencyRate defaultCurrencyRates);
        Task UpdateAsync(DefaultCurrencyRate defaultCurrencyRates);
    }
}
