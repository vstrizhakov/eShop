namespace eShop.Messaging
{
    public interface IMessageHandler<T>
    {
        Task HandleMessageAsync(T message);
    }
}
