using eShop.Database;

namespace eShop.Distribution.Entities
{
    public class Shop : EntityBase
    {
        public string Name { get; set; }

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
