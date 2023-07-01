using Microsoft.EntityFrameworkCore;

namespace eShop.Accounts.Entities
{
    [Index(nameof(TelegramUserId), IsUnique = true)]
    public class Account
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? TelegramUserId { get; set; }

        public Guid? ViberUserId { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
