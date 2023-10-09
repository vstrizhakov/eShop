namespace eShop.Messaging.Models.Distribution.ShopSettings
{
    public class SetShopSettingsFilterRequest : Messaging.Message, IRequest<SetShopSettingsFilterResponse>
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
