using eShopping.Database;
using Telegram.Bot.Types.Enums;

namespace eShopping.Telegram.Entities
{
    public class TelegramChat : EntityBase
    {
        public long ExternalId { get; set; }
        public ChatType Type { get; set; }
        public string? Title { get; set; }
        public Guid? SupergroupId { get; set; }
        public ICollection<EmbeddedTelegramUser> Members { get; set; } = new List<EmbeddedTelegramUser>();
        public TelegramChatSettings Settings { get; set; } = new TelegramChatSettings();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        protected override string GetPartitionKey()
        {
            return UseDiscriminator();
        }
    }
}
