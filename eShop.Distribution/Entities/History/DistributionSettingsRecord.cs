using eShop.Database;

namespace eShop.Distribution.Entities.History
{
    public class DistributionSettingsRecord
    {
        public Guid? PreferredCurrencyId { get; set; }
        public bool ShowSales { get; set; }

        public EmbeddedCurrency? PreferredCurrency { get; set; }
        public ICollection<CurrencyRateRecord> CurrencyRates { get; set; } = new List<CurrencyRateRecord>();
        public ComissionSettingsRecord ComissionSettings { get; set; }
        public ShopSettingsRecord ShopSettings { get; set; }
    }
}
