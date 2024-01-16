namespace eShopping.Catalog.Entities
{
    public class ProductPrice
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Value { get; set; }
        public decimal? DiscountedValue { get; set; }
        public EmbeddedCurrency Currency { get; set; }
    }
}
