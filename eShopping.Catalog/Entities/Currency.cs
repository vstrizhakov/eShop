using eShopping.Database;

namespace eShopping.Catalog.Entities
{
    public class Currency : EntityBase
    {
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public EmbeddedCurrency GenerateEmbedded()
        {
            return new EmbeddedCurrency
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
