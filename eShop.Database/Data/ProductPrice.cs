using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace eShop.Database.Data
{
    [Index(nameof(ProductId), nameof(CurrencyId), IsUnique = true)]
    public class ProductPrice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CurrencyId { get; set; }
        public string ProductId { get; set; }
        public double Value { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        [ValidateNever]
        public Currency Currency { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
    }
}
