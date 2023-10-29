namespace eShop.Messaging.Models.Distribution
{
    public class GetComissionSettingsResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public double Amount { get; }

        public GetComissionSettingsResponse(Guid accountId, double amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }

}
