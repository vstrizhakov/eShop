using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Entities
{
    [Index(nameof(OwnerId))]
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid? CategoryId { get; set; }
        public string Url { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid OwnerId { get; set; }

        public Category? Category { get; set; }
        public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<Composition> Compositions { get; set; } = new List<Composition>();
    }
}
