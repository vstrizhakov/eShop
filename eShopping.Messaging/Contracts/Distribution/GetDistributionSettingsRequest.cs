namespace eShopping.Messaging.Contracts.Distribution
{
    public sealed class GetDistributionSettingsRequest
    {
        public Guid AccountId { get; set; }

        public GetDistributionSettingsRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
