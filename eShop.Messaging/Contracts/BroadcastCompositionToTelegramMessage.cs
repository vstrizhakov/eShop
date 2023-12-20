namespace eShop.Messaging.Contracts
{
    public class BroadcastCompositionToTelegramMessage
    {
        public Guid DistributionId { get; set; }
        public Guid AnnouncerId { get; set; }
        public Guid DistributionItemId { get; set; }
        public Guid TargetId { get; set; }
        public Message Message { get; set; }
    }
}
