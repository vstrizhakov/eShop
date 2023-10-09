namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public class GetShopSettingsRequest : Messaging.Message, IRequest<GetShopSettingsResponse>
    {
        public Guid AccountId { get; }

        public GetShopSettingsRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
