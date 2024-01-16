namespace eShopping.Distribution.Entities
{
    public class DistributionSettings
    {
        public bool ShowSales { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedAt { get; set; }

        public EmbeddedCurrency? PreferredCurrency { get; set; }
        public ICollection<UserCurrencyRate> CurrencyRates { get; set; } = new List<UserCurrencyRate>();
        public ComissionSettings ComissionSettings { get; set; } = new ComissionSettings();
        public ShopSettings ShopSettings { get; set; } = new ShopSettings();
    }
}
