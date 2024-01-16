using eShopping.Catalog.Models.Products;

namespace eShopping.Catalog.Models.Announces
{
    public class CreateAnnounceRequest
    {
        public Guid ShopId { get; set; }

        public IFormFile Image { get; set; }

        public IEnumerable<CreateProductRequest> Products { get; set; }
    }
}
