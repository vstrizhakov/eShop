using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.Entities
{
    [Index(nameof(ExternalId), IsUnique = true)]
    public class TelegramChat
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public long ExternalId { get; set; }
        public ChatType Type { get; set; }
        public string? Title { get; set; }
        public Guid? SupergroupId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<TelegramChatMember> Members { get; set; } = new List<TelegramChatMember>();
        public TelegramChat? Supergroup { get; set; }
        public TelegramChatSettings Settings { get; set; }
    }
}
