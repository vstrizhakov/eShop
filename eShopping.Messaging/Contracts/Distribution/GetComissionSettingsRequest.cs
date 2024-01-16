namespace eShopping.Messaging.Contracts.Distribution
{
    public class GetComissionSettingsRequest
    {
        public Guid AccountId { get; }

        public GetComissionSettingsRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
