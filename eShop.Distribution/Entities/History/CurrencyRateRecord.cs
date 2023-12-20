namespace eShop.Distribution.Entities.History
{
    public class CurrencyRateRecord
    {
        public double Rate { get; set; }
        public EmbeddedCurrency Currency { get; set; }
    }
}
