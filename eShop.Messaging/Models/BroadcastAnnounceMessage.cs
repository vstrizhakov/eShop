namespace eShop.Messaging.Models
{
    public class BroadcastAnnounceMessage : Messaging.Message
    {
        public Guid ProviderId { get; set; }
        public Announce Announce { get; set; }
    }
}
