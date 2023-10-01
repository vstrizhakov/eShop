namespace eShop.Messaging.Models.Distribution
{
    public record GetPreferredCurrencyResponse(Guid AccountId, Currency? PreferredCurrency);
}
