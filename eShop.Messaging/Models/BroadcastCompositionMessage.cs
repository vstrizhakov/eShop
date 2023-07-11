namespace eShop.Messaging.Models
{
    public class BroadcastCompositionMessage
    {
        public Guid ProviderId { get; set; }
        public Composition Composition { get; set; }
    }
}
