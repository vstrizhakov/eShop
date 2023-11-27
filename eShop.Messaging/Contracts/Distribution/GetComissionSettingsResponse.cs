namespace eShop.Messaging.Contracts.Distribution
{
    public class GetComissionSettingsResponse
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
