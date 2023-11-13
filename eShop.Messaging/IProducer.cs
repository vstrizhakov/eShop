namespace eShop.Messaging
{
    public interface IProducer
    {
        void Publish<T>(T message) where T : notnull, IMessage;
    }
}
