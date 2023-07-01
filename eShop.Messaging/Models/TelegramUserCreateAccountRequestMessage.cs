namespace eShop.Messaging.Models
{
    public class TelegramUserCreateAccountRequestMessage
    {
        public Guid TelegramUserId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid ProviderId { get; set; }
    }
}
