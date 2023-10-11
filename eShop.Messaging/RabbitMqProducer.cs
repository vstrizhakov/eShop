﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace eShop.Messaging
{
    internal class RabbitMqProducer : IProducer
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

        public void Publish<T>(T message) where T : notnull
        {
            var data = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(data);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            var routingKey = message.GetType().Name;
            _channel.BasicPublish(
                exchange: Exchange,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);
        }
    }
}
