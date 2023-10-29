namespace eShop.Messaging.Models.Distribution
{
    public class SetComissionAmountRequest : Messaging.Message, IRequest<SetComissionAmountResponse>
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
