namespace eShop.Catalog.Models.Products
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public Guid? CategoryId { get; set; }
        public string Url { get; set; }
        public ProductPrice Price { get; set; }
        public IEnumerable<IFormFile>? Images { get; set; }
    }
}
