using eShopping.Database;

namespace eShopping.Viber.Entities
{
    public class ViberUser : EntityBase
    {
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsSubcribed { get; set; }
        public Guid? AccountId { get; set; }
        public string? ActiveContext { get; set; }

        public ViberChatSettings ChatSettings { get; set; }

        protected override string GetPartitionKey()
        {
            return UseDiscriminator();
        }
    }
}
