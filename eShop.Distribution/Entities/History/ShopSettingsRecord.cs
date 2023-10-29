namespace eShop.Distribution.Entities.History
{
    public class ShopSettingsRecord
    {
        public Guid Id { get; set; }
        public Guid DistributionSettingsId { get; set; }

        public bool Filter { get; set; }
        public ICollection<Shop> PreferredShops { get; set; } = new List<Shop>();

        public DistributionSettingsRecord DistributionSettings { get; set; }
    }
}
