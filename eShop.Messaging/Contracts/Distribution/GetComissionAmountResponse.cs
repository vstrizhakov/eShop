namespace eShop.Messaging.Contracts.Distribution
{
    public class GetComissionAmountResponse
    {
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public GetComissionAmountResponse(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }

}
