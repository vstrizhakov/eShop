using eShop.Messaging.Models.Catalog;

namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public record SetShopSettingsShopStateResponse(Guid AccountId, IEnumerable<Shop> Shops);
}
