namespace eShop.Messaging
{
    public class RabbitMqConsumerQosSettings
    {
        public uint PrefetchSize { get; set; }
        public ushort PrefetchCount { get; set; }
    }
}
