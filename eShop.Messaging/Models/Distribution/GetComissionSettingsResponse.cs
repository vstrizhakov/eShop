namespace eShop.Messaging.Models.Distribution
{
    public record GetComissionSettingsResponse(Guid AccountId, bool Show, decimal Amount);
}
