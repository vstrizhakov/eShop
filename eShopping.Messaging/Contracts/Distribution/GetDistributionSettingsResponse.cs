namespace eShopping.Messaging.Contracts.Distribution
{
    public sealed class GetDistributionSettingsResponse
    {
        public Guid AccountId { get; set; }
        public DistributionSettings DistributionSettings { get; set; }

        public GetDistributionSettingsResponse(Guid accountId, DistributionSettings distributionSettings)
        {
            AccountId = accountId;
            DistributionSettings = distributionSettings;
        }
    }
}
