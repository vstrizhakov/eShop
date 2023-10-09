namespace eShop.Messaging.Models.Distribution
{
    public class GetComissionSettingsRequest : Messaging.Message, IRequest<GetComissionSettingsResponse>
    {
        public Guid AccountId { get; }

        public GetComissionSettingsRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
