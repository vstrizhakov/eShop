using eShop.Catalog.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace eShop.Catalog.Models.Compositions
{
    public class CreateCompositionRequest
    {
        public IFormFile Image { get; set; }

        public IEnumerable<CreateProductRequest> Products { get; set; }
    }
}
