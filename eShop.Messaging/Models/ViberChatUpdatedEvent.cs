namespace eShop.Messaging.Models
{
    public class ViberChatUpdatedEvent : Messaging.Message
    {
        public Guid AccountId { get; set; }
        public Guid ViberUserId { get; set; }
        public bool IsEnabled { get; set; }
    }
}
