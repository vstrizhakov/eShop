namespace eShopping.Distribution.Models.Distributions
{
    public class DistributionRecipient
    {
        public Client Client { get; set; }
        public IEnumerable<DistributionItem> Items { get; set; }
    }
}
