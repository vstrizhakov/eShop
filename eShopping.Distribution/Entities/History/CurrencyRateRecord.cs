namespace eShopping.Distribution.Entities.History
{
    public class CurrencyRateRecord
    {
        public decimal Rate { get; set; }
        public EmbeddedCurrency Currency { get; set; }
    }
}
