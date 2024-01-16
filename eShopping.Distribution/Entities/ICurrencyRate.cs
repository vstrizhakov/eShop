namespace eShopping.Distribution.Entities
{
    public interface ICurrencyRate
    {
        public decimal Rate { get; set; }
        public EmbeddedCurrency SourceCurrency { get; set; }
        public EmbeddedCurrency TargetCurrency { get; set; }
    }
}
