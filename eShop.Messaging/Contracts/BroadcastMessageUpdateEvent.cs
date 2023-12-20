namespace eShop.Messaging.Contracts
{
    public class BroadcastMessageUpdateEvent
    {
        public Guid DistributionId { get; set; }
        public Guid AnnouncerId { get; set; }
        public Guid DistributionItemId { get; set; }
        public bool Succeeded { get; set; }
    }
}
