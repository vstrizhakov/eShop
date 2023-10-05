namespace eShop.Messaging.Models.Distribution
{
    public record SetComissionShowResponse(Guid AccountId, bool Show, decimal Amount);
}
