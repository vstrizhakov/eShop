namespace eShop.Messaging.Models.Distribution
{
    public record GetCurrencyRateResponse(Guid AccountId, bool IsSucceeded, Currency? PreferredCurrency, CurrencyRate? CurrencyRate);
}
