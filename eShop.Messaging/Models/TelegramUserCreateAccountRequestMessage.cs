namespace eShop.Messaging.Models
{
    public class TelegramUserCreateAccountRequestMessage : Messaging.Message
    {
        public Guid TelegramUserId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid ProviderId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
