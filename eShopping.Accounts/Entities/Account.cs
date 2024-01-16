using eShopping.Database;

namespace eShopping.Accounts.Entities
{
    public class Account : EntityBase
    {
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? TelegramUserId { get; set; }

        public Guid? ViberUserId { get; set; }

        public Guid? IdentityUserId { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        protected override string GetPartitionKey()
        {
            return UseDiscriminator();
        }
    }
}
