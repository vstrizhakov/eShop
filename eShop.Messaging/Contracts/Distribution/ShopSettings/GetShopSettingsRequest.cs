namespace eShop.Messaging.Contracts.Distribution.ShopSettings
{
    public class GetShopSettingsRequest
    {
        public Guid AccountId { get; }

        public GetShopSettingsRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
