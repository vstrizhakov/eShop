using eShop.Database;

namespace eShop.Distribution.Entities
{
    public class DefaultCurrencyRate : EntityBase, ICurrencyRate
    {
        public decimal Rate { get; set; }
        public EmbeddedCurrency SourceCurrency { get; set; }
        public EmbeddedCurrency TargetCurrency { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        protected override string GetPartitionKey()
        {
            return UseDiscriminator();
        }
    }
}
