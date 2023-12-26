using eShop.ExchangeRate.Models;

namespace eShop.ExchangeRate
{
    public interface IExchangeRateClient
    {
        Task<GetLatestResponse> GetLatestAsync(string currency);
    }
}