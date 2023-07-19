using eShop.Catalog.Entities;

namespace eShop.Catalog.Models.Compositions
{
    public class Composition
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid? DistributionId { get; set; }
    }
}
