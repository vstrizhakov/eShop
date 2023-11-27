namespace eShop.Messaging.Contracts.Telegram
{
    public class RegisterTelegramUserResponse
    {
        public Guid TelegramUserId { get; set; }
        public Guid AccountId { get; set; }
        public string ProviderEmail { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
