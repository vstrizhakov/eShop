namespace eShop.Messaging.Models.Telegram
{
    public class RegisterTelegramUserResponse : Messaging.Message, IResponse
    {
        public Guid TelegramUserId { get; set; }
        public Guid AccountId { get; set; }
        public string ProviderEmail { get; set; }
    }
}
