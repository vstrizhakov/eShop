namespace eShop.Distribution.Services
{
    public interface IExchangeRateService
    {
        Task<IDictionary<string, float>> GetLatestRatesAsync(string currency);
    }
}
