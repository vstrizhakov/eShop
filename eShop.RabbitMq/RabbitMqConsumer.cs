using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace eShop.RabbitMq
{
    internal class RabbitMqConsumer : IHostedService, IDisposable
    {
        private const string Exchange = "eShop";

        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;
        private readonly IRabbitMqMessageHandler _messageHandler;
        private readonly string _queueName;
        private string? _consumerTag;

        public RabbitMqConsumer(IModel channel, IRabbitMqMessageHandler messageHandler)
        {
            _channel = channel;
            _messageHandler = messageHandler;

            _channel.ExchangeDeclare(
                exchange: Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);

            var result = _channel.QueueDeclare(
                queue: string.Empty,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _queueName = result.QueueName;

            _channel.BasicQos(
                prefetchSize: 0, 
                prefetchCount: 1,
                global: false);

            _channel.QueueBind(
                queue: _queueName,
                exchange: Exchange,
                routingKey: "#",
                arguments: null);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += Consumer_Received;
        }

        public void Dispose()
        {
            _consumer.Received -= Consumer_Received;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumerTag = _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: _consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.BasicCancel(_consumerTag);

            return Task.CompletedTask;
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs args)
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());

            await _messageHandler.HandleMessageAsync(message);

            _channel.BasicAck(args.DeliveryTag, false);
        }
    }
}
