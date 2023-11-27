namespace eShop.Messaging.Contracts
{
    public class BroadcastCompositionToTelegramMessage
    {
        public Guid RequestId { get; set; }
        public Guid TargetId { get; set; }
        public Message Message { get; set; }
    }
}
