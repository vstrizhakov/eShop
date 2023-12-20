namespace eShop.Messaging.Contracts
{
    public class BroadcastAnnounceUpdateEvent
    {
        public Guid AnnounceId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid DistributionId { get; set; }
    }
}
