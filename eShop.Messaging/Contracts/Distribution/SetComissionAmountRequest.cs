namespace eShop.Messaging.Contracts.Distribution
{
    public class SetComissionAmountRequest
    {
        public Guid AccountId { get; }
        public double Amount { get; }

        public SetComissionAmountRequest(Guid accountId, double amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }

}
