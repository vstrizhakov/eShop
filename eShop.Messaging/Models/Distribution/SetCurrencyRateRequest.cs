namespace eShop.Messaging.Models.Distribution
{
    public record SetCurrencyRateRequest(Guid AccountId, Guid CurrencyId, double Rate);
}
