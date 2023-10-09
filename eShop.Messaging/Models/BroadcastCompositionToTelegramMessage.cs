namespace eShop.Messaging.Models
{
    public class BroadcastCompositionToTelegramMessage : Messaging.Message
    {
        public Guid RequestId { get; set; }
        public Guid TargetId { get; set; }
        public Message Message { get; set; }
    }
}
