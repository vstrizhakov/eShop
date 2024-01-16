namespace eShopping.Messaging.Contracts.Distribution.ShopSettings
{
    public class GetShopSettingsShopsRequest
    {
        public Guid AccountId { get; }

        public GetShopSettingsShopsRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
