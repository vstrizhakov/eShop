namespace eShop.Messaging.Models
{
    public class BroadcastCompositionMessage : Messaging.Message
    {
        public Guid ProviderId { get; set; }
        public Composition Composition { get; set; }
    }
}
