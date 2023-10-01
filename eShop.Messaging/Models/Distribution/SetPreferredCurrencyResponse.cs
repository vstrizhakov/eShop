namespace eShop.Messaging.Models.Distribution
{
    public record SetPreferredCurrencyResponse(Guid AccountId, bool Succeeded, Currency? PreferredCurrency);
}
