using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Entities
{
    public class DistributionSettings
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AccountId { get; set; }
        public Guid PreferredCurrencyId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Currency? PreferredCurrency { get; set; }
    }
}
