namespace eShop.Messaging
{
    public interface IMessage
    {
        Guid RequestId { get; set; }
    }
}
