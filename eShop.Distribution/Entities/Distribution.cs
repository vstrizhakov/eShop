namespace eShop.Distribution.Entities
{
    public class Distribution
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }

        public ICollection<DistributionItem> Items { get; set; } = new List<DistributionItem>();
    }
}
