namespace eShop.Catalog.Entities
{
    public class ProductPrice
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid CurrencyId { get; set; }
        public double Value { get; set; }
        public double? DiscountedValue { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Currency Currency { get; set; }
        public Product Product { get; set; }
    }
}
