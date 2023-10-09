namespace eShop.Messaging.Models.Distribution
{
    public class GetComissionAmountResponse : Messaging.Message, IResponse
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
