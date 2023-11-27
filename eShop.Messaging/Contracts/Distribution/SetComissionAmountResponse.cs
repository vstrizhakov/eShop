namespace eShop.Messaging.Contracts.Distribution
{
    public class SetComissionAmountResponse
    {
        public Guid AccountId { get; }
        public double Amount { get; }

        public SetComissionAmountResponse(Guid accountId, double amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }

}
