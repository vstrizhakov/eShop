namespace eShop.Messaging.Models.Distribution
{
    public class SetComissionAmountResponse : Messaging.Message, IResponse
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
