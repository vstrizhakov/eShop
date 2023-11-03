using eShop.Catalog.Models.Products;

namespace eShop.Catalog.Models.Announces
{
    public class CreateAnnounceRequest
    {
        public Guid ShopId { get; set; }

        public IFormFile Image { get; set; }

        public IEnumerable<CreateProductRequest> Products { get; set; }
    }
}
