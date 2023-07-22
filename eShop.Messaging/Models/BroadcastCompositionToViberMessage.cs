namespace eShop.Messaging.Models
{
    public class BroadcastCompositionToViberMessage
    {
        public IEnumerable<DistributionRequest> Requests { get; set; }
        public Message Message { get; set; }
    }
}
