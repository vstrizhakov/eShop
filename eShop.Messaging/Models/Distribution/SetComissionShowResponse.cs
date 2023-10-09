namespace eShop.Messaging.Models.Distribution
{
    public class SetComissionShowResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public bool Show { get; }
        public decimal Amount { get; }

        public SetComissionShowResponse(Guid accountId, bool show, decimal amount)
        {
            AccountId = accountId;
            Show = show;
            Amount = amount;
        }
    }

}
