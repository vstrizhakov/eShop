using eShop.Messaging.Models.Catalog;

namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public record GetShopSettingsShopsResponse(Guid AccountId, IEnumerable<Shop> Shops);
}
