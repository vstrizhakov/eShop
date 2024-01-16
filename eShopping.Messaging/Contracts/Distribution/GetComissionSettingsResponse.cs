namespace eShopping.Messaging.Contracts.Distribution
{
    public class GetComissionSettingsResponse
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
