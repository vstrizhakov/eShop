using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace eShop.Messaging
{
    internal class RabbitMqConsumer<T> : IConsumer, IDisposable
    {
        private const string Exchange = "eShop";

        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitMqConsumerOptions<T> _options;

        private readonly EventingBasicConsumer _consumer;

        private string? _consumerTag;

        public RabbitMqConsumer(IModel channel, IServiceScopeFactory serviceScopeFactory, IOptions<RabbitMqConsumerOptions<T>> options)
        {
            _channel = channel;
            _serviceScopeFactory = serviceScopeFactory;
            _options = options.Value;

            _channel.ExchangeDeclare(
                exchange: Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);

            var queueName = _options.QueueName;

            var result = _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);

            _channel.QueueBind(
                queue: queueName,
                exchange: Exchange,
                routingKey: _options.RoutingKey,
                arguments: null);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += Consumer_Received;
        }

        public void Dispose()
        {
            _consumer.Received -= Consumer_Received;
        }

        public void Start()
        {
            _consumerTag = _channel.BasicConsume(
                queue: _options.QueueName,
                autoAck: false,
                consumer: _consumer);
        }

        public void Stop()
        {
            _channel.BasicCancel(_consumerTag);
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs args)
        {
            var data = Encoding.UTF8.GetString(args.Body.ToArray());
            var message = JsonConvert.DeserializeObject<T>(data);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var messageHandler = scope.ServiceProvider.GetRequiredService<IMessageHandler<T>>();

                await messageHandler.HandleMessageAsync(message);
            }

            _channel.BasicAck(args.DeliveryTag, false);
        }
    }
}
