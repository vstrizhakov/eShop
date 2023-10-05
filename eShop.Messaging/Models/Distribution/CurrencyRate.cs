namespace eShop.Messaging.Models.Distribution
{
    public class CurrencyRate
    {
        public bool IsDefault { get; set; }
        public Currency Currency { get; set; }
        public double Rate { get; set; }
    }
}