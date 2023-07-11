namespace eShop.Messaging.Models
{
    public class BroadcastCompositionUpdateEvent
    {
        public Guid CompositionId { get; set; }
        public Guid DistributionGroupId { get; set; }
    }
}
