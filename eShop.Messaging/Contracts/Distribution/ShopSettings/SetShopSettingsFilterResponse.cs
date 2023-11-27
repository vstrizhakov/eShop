namespace eShop.Messaging.Contracts.Distribution.ShopSettings
{
    public class SetShopSettingsFilterResponse
    {
        public Guid AccountId { get; }
        public ShopSettings ShopSettings { get; }

        public SetShopSettingsFilterResponse(Guid accountId, ShopSettings shopSettings)
        {
            AccountId = accountId;
            ShopSettings = shopSettings;
        }
    }

}
