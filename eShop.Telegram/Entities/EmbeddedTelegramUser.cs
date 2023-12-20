using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.Entities
{
    public class EmbeddedTelegramUser
    {
        public Guid UserId { get; set; }
        public ChatMemberStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
