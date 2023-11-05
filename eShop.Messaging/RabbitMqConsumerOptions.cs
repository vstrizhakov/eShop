namespace eShop.Messaging
{
    public class RabbitMqConsumerOptions<T>
    {
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
        public RabbitMqConsumerQosSettings QoS { get; set; } = new RabbitMqConsumerQosSettings
        {
            PrefetchSize = 0,
            PrefetchCount = 100,
        };
    }
}
