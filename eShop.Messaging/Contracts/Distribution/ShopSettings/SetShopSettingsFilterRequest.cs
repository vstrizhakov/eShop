namespace eShop.Messaging.Contracts.Distribution.ShopSettings
{
    public class SetShopSettingsFilterRequest
    {
        public Guid AccountId { get; }
        public bool Filter { get; }

        public SetShopSettingsFilterRequest(Guid accountId, bool filter)
        {
            AccountId = accountId;
            Filter = filter;
        }
    }

}
