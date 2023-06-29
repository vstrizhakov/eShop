using Microsoft.EntityFrameworkCore;

namespace eShop.Database.Data
{
    [PrimaryKey(nameof(TelegramChatId), nameof(OwnerId))]
    public class TelegramChatSettings
    {
        public string TelegramChatId { get; set; }
        public string OwnerId { get; set; }
        public bool IsEnabled { get; set; }

        public TelegramChat TelegramChat { get; set; }
        public User Owner { get; set; }
    }
}
