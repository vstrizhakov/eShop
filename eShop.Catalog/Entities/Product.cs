using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Entities
{
    [Index(nameof(OwnerId))]
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? CategoryId { get; set; }
        public Uri Url { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid OwnerId { get; set; }
        public string? Description { get; set; }

        public Category? Category { get; set; }
        public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<Announce> Announces { get; set; } = new List<Announce>();
    }
}
