namespace eShop.Messaging.Models
{
    public class TelegramChatUpdatedEvent
    {
        public Guid AccountId { get; set; }
        public Guid TelegramChatId { get; set; }
        public bool IsEnabled { get; set; }
    }
}
