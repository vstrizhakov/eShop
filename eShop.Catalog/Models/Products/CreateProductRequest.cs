using System.ComponentModel.DataAnnotations;

namespace eShop.Catalog.Models.Products
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public Guid? CategoryId { get; set; }

        [Url]
        public string Url { get; set; }
        
        public CreateProductPrice Price { get; set; }
        public IEnumerable<IFormFile>? Images { get; set; }
        public string? Description { get; set; }
    }
}
