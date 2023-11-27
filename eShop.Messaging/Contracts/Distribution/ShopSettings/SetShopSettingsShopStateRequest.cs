namespace eShop.Messaging.Contracts.Distribution.ShopSettings
{
    public class SetShopSettingsShopStateRequest
    {
        public Guid AccountId { get; }
        public Guid ShopId { get; }
        public bool IsEnabled { get; }

        public SetShopSettingsShopStateRequest(Guid accountId, Guid shopId, bool isEnabled)
        {
            AccountId = accountId;
            ShopId = shopId;
            IsEnabled = isEnabled;
        }
    }

}
