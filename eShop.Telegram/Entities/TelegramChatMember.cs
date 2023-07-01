using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.Entities
{
    [PrimaryKey(nameof(UserId), nameof(ChatId))]
    public class TelegramChatMember
    {
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }

        public ChatMemberStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public TelegramUser User { get; set; }
        public TelegramChat Chat { get; set; }
    }
}
