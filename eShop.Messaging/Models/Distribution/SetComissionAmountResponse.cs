namespace eShop.Messaging.Models.Distribution
{
    public class SetComissionAmountResponse : Messaging.Message, IResponse
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
