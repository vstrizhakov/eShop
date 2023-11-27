namespace eShop.Messaging.Contracts.Distribution
{
    public class GetComissionAmountResponse
    {
        public Guid AccountId { get; }
        public double Amount { get; }

        public GetComissionAmountResponse(Guid accountId, double amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }

}
