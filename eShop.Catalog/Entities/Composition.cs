using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Entities
{
    [Index(nameof(OwnerId))]
    public class Composition
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid OwnerId { get; set; }
        public Guid? DistributionGroupId { get; set; }
        public Guid ShopId { get; set; }

        public ICollection<CompositionImage> Images { get; set; } = new List<CompositionImage>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public Shop Shop { get; set; }
    }
}
