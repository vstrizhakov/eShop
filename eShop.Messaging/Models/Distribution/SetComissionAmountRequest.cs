namespace eShop.Messaging.Models.Distribution
{
    public record SetComissionAmountRequest(Guid AccountId, decimal Amount);
}
