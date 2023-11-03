namespace eShop.Distribution.Models.Distributions
{
    public class Distribution
    {
        public Guid Id { get; set; }
        public IEnumerable<DistributionRecipient> Recipients { get; set; }
    }
}
