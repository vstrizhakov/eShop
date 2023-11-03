using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Entities
{
    [Index(nameof(OwnerId))]
    public class Announce
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid OwnerId { get; set; }
        public Guid? DistributionId { get; set; }
        public Guid ShopId { get; set; }

        public ICollection<AnnounceImage> Images { get; set; } = new List<AnnounceImage>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public Shop Shop { get; set; }
    }
}
