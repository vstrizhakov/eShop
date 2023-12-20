namespace eShop.Catalog.Entities
{
    public class ProductPrice
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public double Value { get; set; }
        public double? DiscountedValue { get; set; }
        public EmbeddedCurrency Currency { get; set; }
    }
}
