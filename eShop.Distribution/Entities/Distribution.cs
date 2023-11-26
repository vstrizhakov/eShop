namespace eShop.Distribution.Entities
{
    public class Distribution
    {
        public Guid Id { get; set; }
        public Guid AnnouncerId { get; set; }

        public ICollection<DistributionItem> Items { get; set; } = new List<DistributionItem>();
    }
}
