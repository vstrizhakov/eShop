using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Entities
{
    [Index(nameof(OwnerId))]
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    }
}
