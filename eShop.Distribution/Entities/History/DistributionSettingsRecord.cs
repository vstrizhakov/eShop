namespace eShop.Distribution.Entities.History
{
    public class DistributionSettingsRecord
    {
        public Guid Id { get; set; }
        public Guid PreferredCurrencyId { get; set; }
        public bool ShowSales { get; set; }

        public Currency? PreferredCurrency { get; set; }
        public ICollection<CurrencyRateRecord> CurrencyRates { get; set; } = new List<CurrencyRateRecord>();
        public ComissionSettingsRecord ComissionSettings { get; set; }
        public ShopSettingsRecord ShopSettings { get; set; }
    }
}
