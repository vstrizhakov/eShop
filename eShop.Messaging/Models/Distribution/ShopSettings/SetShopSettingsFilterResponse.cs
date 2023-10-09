namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public class SetShopSettingsFilterResponse : Messaging.Message, IResponse
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
