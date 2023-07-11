namespace eShop.Messaging.Models
{
    public class BroadcastCompositionToViberMessage
    {
        public Guid DistributionGroupId { get; set; }
        public IEnumerable<Guid> ViberChatIds { get; set; }
        public Uri Image { get; set; }
        public string Caption { get; set; }
    }
}
