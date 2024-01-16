using eShopping.ExchangeRate.Models;

namespace eShopping.ExchangeRate
{
    public interface IExchangeRateClient
    {
        Task<GetLatestResponse> GetLatestAsync(string currency);
    }
}