using eShopping.Database;

namespace eShopping.Identity.Entities
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
