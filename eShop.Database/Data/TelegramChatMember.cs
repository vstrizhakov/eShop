using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace eShop.Database.Data
{
    [PrimaryKey(nameof(UserId), nameof(ChatId))]
    public class TelegramChatMember
    {
        public string UserId { get; set; }
        public string ChatId { get; set; }

        public ChatMemberStatus Status { get; set; }

        public TelegramUser User { get; set; }
        public TelegramChat Chat { get; set; }
    }
}
