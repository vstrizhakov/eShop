namespace eShopping.Messaging.Contracts.Distribution.ShopSettings
{
    public class GetShopSettingsShopsResponse
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
