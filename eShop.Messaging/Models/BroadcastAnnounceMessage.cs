namespace eShop.Messaging.Models
{
    public class BroadcastAnnounceMessage : Messaging.Message
    {
        public Guid AnnouncerId { get; set; }
        public Announce Announce { get; set; }
    }
}
