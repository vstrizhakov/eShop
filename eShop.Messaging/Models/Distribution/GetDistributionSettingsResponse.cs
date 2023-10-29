namespace eShop.Messaging.Models.Distribution
{
    public sealed class GetDistributionSettingsResponse : Messaging.Message, IResponse
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
