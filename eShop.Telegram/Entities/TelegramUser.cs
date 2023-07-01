using Microsoft.EntityFrameworkCore;

namespace eShop.Telegram.Entities
{
    [Index(nameof(ExternalId), IsUnique = true)]
    public class TelegramUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Username { get; set; }

        public long ExternalId { get; set; }

        public Guid? AccountId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<TelegramChatMember> Chats { get; set; } = new List<TelegramChatMember>();
        public ICollection<TelegramChatSettings> ChatSettings { get; set; } = new List<TelegramChatSettings>();
    }
}
