namespace eShop.Messaging.Models
{
    public class ViberChatUpdatedEvent
    {
        public Guid AccountId { get; set; }
        public Guid ViberUserId { get; set; }
        public bool IsEnabled { get; set; }
    }
}
