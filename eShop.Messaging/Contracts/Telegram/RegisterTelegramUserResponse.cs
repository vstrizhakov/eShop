using eShop.Messaging.Contracts.Distribution;

namespace eShop.Messaging.Contracts.Telegram
{
    public class RegisterTelegramUserResponse
    {
        public Guid TelegramUserId { get; set; }
        public Guid AccountId { get; set; }
        public Announcer? Announcer { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
