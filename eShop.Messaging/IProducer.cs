namespace eShop.Messaging
{
    public interface IProducer
    {
        void Publish<T>(T message);
    }
}
