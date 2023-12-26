namespace eShop.Distribution.Entities
{
    public interface ICurrencyRate
    {
        public float Rate { get; set; }
        public EmbeddedCurrency SourceCurrency { get; set; }
        public EmbeddedCurrency TargetCurrency { get; set; }
    }
}
