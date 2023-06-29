using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace eShop.Database.Data
{
    [Index(nameof(ExternalId), IsUnique = true)]
    public class TelegramChat
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public long ExternalId { get; set; }
        public ChatType Type { get; set; }
        public string? Title { get; set; }
        public string? SupergroupId { get; set; }

        public ICollection<TelegramChatMember> Members { get; set; } = new List<TelegramChatMember>();
        public TelegramChat? Supergroup { get; set; }
        public TelegramChatSettings Settings { get; set; }
    }
}
