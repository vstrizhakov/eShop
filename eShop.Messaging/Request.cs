namespace eShop.Messaging
{
    public class Message : IMessage
    {
        public Guid RequestId { get; set; }
    }
}
