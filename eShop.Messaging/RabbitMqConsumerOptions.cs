namespace eShop.Messaging
{
    public class RabbitMqConsumerOptions<T>
    {
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
    }
}
