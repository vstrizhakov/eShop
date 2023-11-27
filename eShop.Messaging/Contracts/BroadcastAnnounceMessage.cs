namespace eShop.Messaging.Contracts
{
    public class BroadcastAnnounceMessage
    {
        public Guid AnnouncerId { get; set; }
        public Announce Announce { get; set; }
    }
}
