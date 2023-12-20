using eShop.Database;

namespace eShop.Distribution.Entities
{
    public class Currency : EntityBase
    {
        public string Name { get; set; }

        public EmbeddedCurrency GeneratedEmbedded()
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
