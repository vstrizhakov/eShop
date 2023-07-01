using Microsoft.EntityFrameworkCore;

namespace eShop.Telegram.Entities
{
    [PrimaryKey(nameof(TelegramChatId), nameof(OwnerId))]
    public class TelegramChatSettings
    {
        public Guid TelegramChatId { get; set; }
        public Guid OwnerId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public TelegramChat TelegramChat { get; set; }
        public TelegramUser Owner { get; set; }
    }
}
