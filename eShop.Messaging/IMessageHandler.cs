namespace eShop.Messaging
{
    public interface IMessageHandler<T> where T : notnull, IMessage
    {
        Task HandleMessageAsync(T message);
    }
}
