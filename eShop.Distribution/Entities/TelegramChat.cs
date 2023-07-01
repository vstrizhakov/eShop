using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Entities
{
    [PrimaryKey(nameof(AccountId), nameof(TelegramChatId))]
    public class TelegramChat
    {
        public Guid AccountId { get; set; }
        public Guid TelegramChatId { get; set; }
        public bool IsEnabled { get; set; }

        public Account Account { get; set; }
    }
}
