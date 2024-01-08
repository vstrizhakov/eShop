namespace eShop.Messaging.Contracts.Distribution
{
    public class SetComissionAmountRequest
    {
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public SetComissionAmountRequest(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }

}
