namespace eShop.Catalog.Entities
{
    public class ProductImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Path { get; set; }
    }
}
