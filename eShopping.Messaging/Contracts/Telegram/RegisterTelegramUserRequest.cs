namespace eShopping.Messaging.Contracts.Telegram
{
    public class RegisterTelegramUserRequest
    {
        public Guid TelegramUserId { get; set; }
        public Guid? AnnouncerId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
