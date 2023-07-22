namespace eShop.Messaging.Models
{
    public class BroadcastCompositionToTelegramMessage
    {
        public IEnumerable<DistributionRequest> Requests { get; set; }
        public Message Message { get; set; }
    }
}
