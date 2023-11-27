namespace eShop.Messaging.Contracts.Distribution
{
    public sealed class SetShowSalesResponse
    {
        public Guid AccountId { get; set; }
        public DistributionSettings DistributionSettings { get; set; }

        public SetShowSalesResponse(Guid accountId, DistributionSettings distributionSettings)
        {
            AccountId = accountId;
            DistributionSettings = distributionSettings;
        }
    }
}