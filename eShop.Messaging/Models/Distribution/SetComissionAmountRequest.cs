namespace eShop.Messaging.Models.Distribution
{
    public class SetComissionAmountRequest : Messaging.Message, IRequest<SetComissionAmountResponse>
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
