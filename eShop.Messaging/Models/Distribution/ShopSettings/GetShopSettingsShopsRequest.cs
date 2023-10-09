namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public class GetShopSettingsShopsRequest : Messaging.Message, IRequest<GetShopSettingsShopsResponse>
    {
        public Guid AccountId { get; }

        public GetShopSettingsShopsRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
