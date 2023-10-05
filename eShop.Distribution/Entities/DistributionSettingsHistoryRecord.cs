namespace eShop.Distribution.Entities
{
    public class DistributionSettingsHistoryRecord
    {
        public Guid Id { get; set; }
        public Guid PreferredCurrencyId { get; set; }

        public Currency? PreferredCurrency { get; set; }
        public ICollection<CurrencyRateHistoryRecord> CurrencyRates { get; set; } = new List<CurrencyRateHistoryRecord>();
    }
}
