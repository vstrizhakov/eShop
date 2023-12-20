using eShop.Database;

namespace eShop.Identity.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        protected override string GetPartitionKey()
        {
            return UseDiscriminator();
        }
    }
}
