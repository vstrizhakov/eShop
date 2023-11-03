namespace eShop.Catalog.Entities
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public Guid ProductId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Product Product { get; set; }
    }
}
