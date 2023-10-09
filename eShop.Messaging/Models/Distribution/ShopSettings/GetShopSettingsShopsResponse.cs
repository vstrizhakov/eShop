using eShop.Messaging.Models.Catalog;

namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public class GetShopSettingsShopsResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public IEnumerable<Shop> Shops { get; }

        public GetShopSettingsShopsResponse(Guid accountId, IEnumerable<Shop> shops)
        {
            AccountId = accountId;
            Shops = shops;
        }
    }

}
