namespace eShop.Messaging.Models.Distribution
{
    public class GetComissionAmountResponse : Messaging.Message, IResponse
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
