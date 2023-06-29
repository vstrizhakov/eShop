using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace eShop.Database.Data
{
    public class Currency
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
