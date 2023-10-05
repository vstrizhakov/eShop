using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Entities
{
    [PrimaryKey(nameof(AccountId))]
    public class DistributionSettings
    {
        public Guid AccountId { get; set; }

        public Guid? PreferredCurrencyId { get; set; }
        public bool ShowSales { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedAt { get; set; }

        public Currency? PreferredCurrency { get; set; }
        public ICollection<CurrencyRate> CurrencyRates { get; set; } = new List<CurrencyRate>();
        public ComissionSettings ComissionSettings { get; set; }
        public ShopSettings ShopSettings { get; set; }
    }
}
