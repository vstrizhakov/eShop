using Telegram.Bot.Types.Enums;

namespace eShopping.Telegram.Entities
{
    public class EmbeddedTelegramChat
    {
        public Guid ChatId { get; set; }
        public ChatType Type { get; set; }
        public ChatMemberStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
