namespace eShop.Messaging.Models.Distribution
{
    public sealed class SetShowSalesResponse : Messaging.Message, IResponse
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