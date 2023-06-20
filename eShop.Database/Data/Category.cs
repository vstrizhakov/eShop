using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace eShop.Database.Data
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string? ParentCategoryId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        [ValidateNever]
        public string OwnerId { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public User Owner { get; set; }
    }
}
