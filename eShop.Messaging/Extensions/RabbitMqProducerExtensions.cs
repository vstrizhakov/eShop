using eShop.RabbitMq;
using Newtonsoft.Json;

namespace eShop.Messaging.Extensions
{
    public static class RabbitMqProducerExtensions
    {
        public static void Publish<T>(this IRabbitMqProducer producer, T message)
        {
            var internalMessage = new Message
            {
                Name = message.GetType().Name,
                Data = message,
            };
            var body = JsonConvert.SerializeObject(internalMessage);
            producer.Publish(body);
        }
    }
}