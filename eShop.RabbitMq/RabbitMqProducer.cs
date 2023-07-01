using RabbitMQ.Client;
using System.Text;

namespace eShop.RabbitMq
{
    internal class RabbitMqProducer : IRabbitMqProducer
    {
        private const string Exchange = "eShop";

        private readonly IModel _channel;

        public RabbitMqProducer(IModel channel)
        {
            _channel = channel;

            _channel.ExchangeDeclare(
                exchange: Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);
        }

        public void Publish(string message, string routingKey = "")
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: Exchange,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);
        }
    }
}
