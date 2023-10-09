namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public class SetShopSettingsShopStateRequest : Messaging.Message, IRequest<SetShopSettingsShopStateResponse>
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
