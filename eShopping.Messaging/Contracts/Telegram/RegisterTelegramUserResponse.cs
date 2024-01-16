using eShopping.Messaging.Contracts.Distribution;

namespace eShopping.Messaging.Contracts.Telegram
{
    public class RegisterTelegramUserResponse
    {
        public Guid TelegramUserId { get; set; }
        public Guid AccountId { get; set; }
        public Announcer? Announcer { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
