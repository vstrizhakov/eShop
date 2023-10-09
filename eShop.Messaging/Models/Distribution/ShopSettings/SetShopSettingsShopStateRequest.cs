namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public record SetShopSettingsShopStateRequest(Guid AccountId, Guid ShopId, bool IsEnabled);
}
