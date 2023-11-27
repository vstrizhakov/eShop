namespace eShop.Messaging.Contracts
{
    public class BroadcastMessageUpdateEvent
    {
        public Guid RequestId { get; set; }
        public bool Succeeded { get; set; }
    }
}
