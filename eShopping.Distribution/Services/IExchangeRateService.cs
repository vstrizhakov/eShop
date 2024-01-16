namespace eShopping.Distribution.Services
{
    public interface IExchangeRateService
    {
        Task<IDictionary<string, decimal>> GetLatestRatesAsync(string currency);
    }
}
