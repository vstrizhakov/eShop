namespace eShop.Messaging.Contracts.Distribution
{
    public class SetComissionAmountResponse
    {
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public SetComissionAmountResponse(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }

}
