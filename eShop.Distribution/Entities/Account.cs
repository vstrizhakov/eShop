using eShop.Database;

namespace eShop.Distribution.Entities
{
    public class Account : EntityBase
    {
        public Guid? TelegramUserId { get; set; }
        public Guid? ViberUserId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActivated { get; set; }
        public Guid? AnnouncerId { get; set; }

        public ICollection<TelegramChat> TelegramChats { get; set; } = new List<TelegramChat>();
        public ViberChat? ViberChat { get; set; }
        public DistributionSettings DistributionSettings { get; set; } = new DistributionSettings();

        public EmbeddedAccount GeneratedEmbedded()
        {
            return new EmbeddedAccount
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
            };
        }

        protected override string GetPartitionKey()
        {
            return UseDiscriminator();
        }
    }
}
