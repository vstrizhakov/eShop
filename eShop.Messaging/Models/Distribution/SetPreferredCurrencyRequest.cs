namespace eShop.Messaging.Models.Distribution
{
    public record SetPreferredCurrencyRequest(Guid AccountId, Guid CurrencyId);
}
