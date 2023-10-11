namespace eShop.Messaging.Models.Distribution
{
    public class GetComissionSettingsResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public GetComissionSettingsResponse(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }

}
