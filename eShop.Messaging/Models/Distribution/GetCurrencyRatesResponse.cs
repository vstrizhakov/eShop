namespace eShop.Messaging.Models.Distribution
{
    public record GetCurrencyRatesResponse(Guid AccountId, Currency PreferredCurrency, IEnumerable<CurrencyRate> CurrencyRates);
}
