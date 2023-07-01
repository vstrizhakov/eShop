namespace eShop.RabbitMq
{
    public interface IRabbitMqProducer
    {
        void Publish(string message, string routingKey = "");
    }
}
