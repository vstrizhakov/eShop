using eShop.Catalog.Models.Products;

namespace eShop.Catalog.Models.Compositions
{
    public class CreateCompositionRequest
    {
        public Guid ShopId { get; set; }

        public IFormFile Image { get; set; }

        public IEnumerable<CreateProductRequest> Products { get; set; }
    }
}
