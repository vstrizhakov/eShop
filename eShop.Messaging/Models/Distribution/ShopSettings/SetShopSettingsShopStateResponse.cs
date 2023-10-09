using eShop.Messaging.Models.Catalog;

namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public class SetShopSettingsShopStateResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public IEnumerable<Shop> Shops { get; }

        public SetShopSettingsShopStateResponse(Guid accountId, IEnumerable<Shop> shops)
        {
            AccountId = accountId;
            Shops = shops;
        }
    }

}
