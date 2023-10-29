namespace eShop.Distribution.Entities.History
{
    public class ComissionSettingsRecord
    {
        public Guid Id { get; set; }
        public Guid DistributionSettingsId { get; set; }

        public double Amount { get; set; }

        public DistributionSettingsRecord DistributionSettings { get; set; }
    }
}
