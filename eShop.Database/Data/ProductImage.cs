namespace eShop.Database.Data
{
    public class ProductImage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Path { get; set; }
        public string ProductId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Product Product { get; set; }
    }
}
