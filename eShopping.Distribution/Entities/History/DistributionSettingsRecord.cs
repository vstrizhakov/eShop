using eShopping.Database;
using eShopping.Distribution.Entities;

namespace eShopping.Distribution.Entities.History
{
    public class DistributionSettingsRecord
    {
        public bool ShowSales { get; set; }

        public EmbeddedCurrency? PreferredCurrency { get; set; }
        public ICollection<CurrencyRateRecord> CurrencyRates { get; set; } = new List<CurrencyRateRecord>();
        public ComissionSettingsRecord ComissionSettings { get; set; }
        public ShopSettingsRecord ShopSettings { get; set; }
    }
}
