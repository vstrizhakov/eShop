using eShop.Database;

namespace eShop.Catalog.Entities
{
    public class Announce : EntityBase
    {
        public Guid OwnerId { get; set; }
        public Guid? DistributionId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public ICollection<AnnounceImage> Images { get; set; } = new List<AnnounceImage>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public EmbeddedShop Shop { get; set; }

        protected override string GetPartitionKey()
        {
            return OwnerId.ToString();
        }
    }
}
