namespace eShop.Messaging.Models
{
    public class BroadcastCompositionUpdateEvent : Messaging.Message
    {
        public Guid CompositionId { get; set; }
        public Guid DistributionGroupId { get; set; }
    }
}
