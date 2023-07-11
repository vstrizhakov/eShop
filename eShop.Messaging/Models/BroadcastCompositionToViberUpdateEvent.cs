namespace eShop.Messaging.Models
{
    public class BroadcastCompositionToViberUpdateEvent
    {
        public Guid DistributionGroupId { get; set; }
        public Guid ViberChatId { get; set; }
        public bool Succeeded { get; set; }
    }
}
