namespace eShop.Distribution.Entities
{
    public class ComissionSettings
    {
        public Guid Id { get; set; }
        public Guid? DistributionSettingsId { get; set; }

        public bool Show { get; set; }
        public decimal Amount { get; set; }

        public DistributionSettings? DistributionSettings { get; set; }
    }
}
