using eShop.Catalog.Models.Products;
using eShop.Catalog.Models.Shops;

namespace eShop.Catalog.Models.Announces
{
    public class Announce
    {
        public Guid Id { get; set; }
        public Guid? DistributionId { get; set; }
        public Shop Shop { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<string> Images { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
