namespace eShop.Distribution.Models
{
    public class Distribution
    {
        public Guid Id { get; set; }
        public IEnumerable<DistributionItem> Items { get; set; }
    }
}
