namespace eShop.Messaging.Contracts.Distribution.ShopSettings
{
    public class GetShopSettingsResponse
    {
        public Guid AccountId { get; }
        public ShopSettings ShopSettings { get; }

        public GetShopSettingsResponse(Guid accountId, ShopSettings shopSettings)
        {
            AccountId = accountId;
            ShopSettings = shopSettings;
        }
    }

}
