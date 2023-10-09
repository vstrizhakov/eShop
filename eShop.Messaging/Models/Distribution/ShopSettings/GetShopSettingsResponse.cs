namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public class GetShopSettingsResponse : Messaging.Message, IResponse
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
