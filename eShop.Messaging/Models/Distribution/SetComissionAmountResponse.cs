namespace eShop.Messaging.Models.Distribution
{
    public record SetComissionAmountResponse(Guid AccountId, bool Show, decimal Amount);
}
