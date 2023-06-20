using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace eShop.Database.Data
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string? CategoryId { get; set; }
        public string Url { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        [ValidateNever]
        public string OwnerId { get; set; }

        [ValidateNever]
        public Category? Category { get; set; }
        public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<Composition> Compositions { get; set; } = new List<Composition>();

        public User Owner { get; set; }
    }
}
