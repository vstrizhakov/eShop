namespace eShop.Distribution.Entities
{
    public class DistributionGroup
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProviderId { get; set; }

        public ICollection<DistributionGroupItem> Items { get; set; } = new List<DistributionGroupItem>();
    }
}
