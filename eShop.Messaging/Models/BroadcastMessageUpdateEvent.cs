namespace eShop.Messaging.Models
{
    public class BroadcastMessageUpdateEvent : Messaging.Message
    {
        public Guid RequestId { get; set; }
        public bool Succeeded { get; set; }
    }
}
