using eShop.Database;

namespace eShop.Catalog.Entities
{
    public class Shop : EntityBase
    {
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public EmbeddedShop GenerateEmbedded()
        {
            return new EmbeddedShop
            {
                Id = Id,
                Name = Name,
            };
        }

        protected override string GetPartitionKey()
        {
            return UseDiscriminator();
        }
    }
}
