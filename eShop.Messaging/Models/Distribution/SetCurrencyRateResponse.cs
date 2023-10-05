namespace eShop.Messaging.Models.Distribution
{
    public record SetCurrencyRateResponse(Guid AccountId, Currency PreferredCurrency, IEnumerable<CurrencyRate> CurrencyRates);
}
