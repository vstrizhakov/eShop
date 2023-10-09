namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public record SetShopSettingsFilterRequest(Guid AccountId, bool Filter);
}
