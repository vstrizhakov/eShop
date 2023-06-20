using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace eShop.Database.Data
{
    public class Composition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        [ValidateNever]
        public string OwnerId { get; set; }

        public ICollection<CompositionImage> Images { get; set; } = new List<CompositionImage>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public User Owner { get; set; }
    }
}
