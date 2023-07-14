namespace eShop.RabbitMq
{
    public interface IProducer
    {
        void Publish<T>(T message);
    }
}
