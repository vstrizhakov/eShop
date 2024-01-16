namespace eShopping.Messaging.Contracts.Distribution.ShopSettings
{
    public class SetShopSettingsShopStateResponse
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
