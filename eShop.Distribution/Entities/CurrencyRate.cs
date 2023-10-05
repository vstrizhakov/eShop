using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Entities
{
    [Index(nameof(DistributionSettingsId), nameof(TargetCurrencyId), nameof(SourceCurrencyId), IsUnique = true)]
    public class CurrencyRate
    {
        public Guid Id { get; set; }
        public Guid? DistributionSettingsId { get; set; }
        public Guid TargetCurrencyId { get; set; }
        public Guid SourceCurrencyId { get; set; }

        public double Rate { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedAt { get; set; }

        public Currency SourceCurrency { get; set; }
        public Currency TargetCurrency { get; set; }
        public DistributionSettings? DistributionSettings { get; set; }
    }
}
