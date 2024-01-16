using eShopping.Distribution.Entities.History;

namespace eShopping.Distribution.Entities
{
    public class DistributionGroup
    {
        public EmbeddedAccount Account { get; set; }
        public ICollection<DistributionItem> Items { get; set; } = new List<DistributionItem>();
        public DistributionSettingsRecord DistributionSettings { get; set; }
    }
}
