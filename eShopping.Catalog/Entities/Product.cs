namespace eShopping.Catalog.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Uri Url { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
