namespace eShop.Messaging.Contracts.Distribution
{
    public class GetComissionAmountRequest
    {
        public Guid AccountId { get; }

        public GetComissionAmountRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
