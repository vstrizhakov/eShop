namespace eShop.Messaging.Models
{
    public class BroadcastAnnounceUpdateEvent : Messaging.Message
    {
        public Guid AnnounceId { get; set; }
        public Guid DistributionId { get; set; }
    }
}
