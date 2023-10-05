namespace eShop.Distribution.Entities
{
    public class ShopSettings
    {
        public Guid Id { get; set; }
        public Guid? DistributionSettingsId { get; set; }

        public bool Filter { get; set; }
        public ICollection<Shop> PreferredShops { get; set; } = new List<Shop>();

        public DistributionSettings? DistributionSettings { get; set; }
    }
}
