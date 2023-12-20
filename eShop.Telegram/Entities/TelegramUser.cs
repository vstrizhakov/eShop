using eShop.Database;

namespace eShop.Telegram.Entities
{
    public class TelegramUser : EntityBase
    {
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Username { get; set; }

        public string? PhoneNumber { get; set; }

        public long ExternalId { get; set; }

        public string? ActiveContext { get; set; }

        public Guid? AccountId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<EmbeddedTelegramChat> Chats { get; set; } = new List<EmbeddedTelegramChat>();

        protected override string GetPartitionKey()
        {
            return UseDiscriminator();
        }
    }
}
