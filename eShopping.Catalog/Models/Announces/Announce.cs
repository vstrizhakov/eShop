using eShopping.Catalog.Models.Products;
using eShopping.Catalog.Models.Shops;

namespace eShopping.Catalog.Models.Announces
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
