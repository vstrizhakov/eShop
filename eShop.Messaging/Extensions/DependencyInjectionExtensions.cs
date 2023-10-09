using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Reflection;

namespace eShop.Messaging.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMessageHandler<TMessage, THandler>(
            this IServiceCollection services) where THandler : class, IMessageHandler<TMessage>
        {
            services.AddHostedService<MessagingManager>();
            services.Configure<RabbitMqConsumerOptions<TMessage>>(options =>
            {
                var name = typeof(TMessage).Name;
                options.QueueName = name;
                options.RoutingKey = name;
            });
            services.AddScoped<IMessageHandler<TMessage>, THandler>();
            services.AddSingleton<IConsumer, RabbitMqConsumer<TMessage>>();

            return services;
        }

        public static IServiceCollection AddRabbitMqProducer(this IServiceCollection services)
        {
            services.AddSingleton<IProducer, RabbitMqProducer>();

            return services;
        }

        public static IServiceCollection AddRabbitMq(this IServiceCollection services, Action<RabbitMqOptions> configure)
        {
            services.Configure(configure);
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<RabbitMqOptions>>();
                var factory = new ConnectionFactory
                {
                    HostName = options.Value.HostName,
                };
                return factory;
            });
            services.AddSingleton(serviceProvider =>
            {
                var factory = serviceProvider.GetRequiredService<IConnectionFactory>();
                var connection = factory.CreateConnection();
                return connection;
            });
            services.AddSingleton(serviceProvider =>
            {
                var connection = serviceProvider.GetRequiredService<IConnection>();
                var channel = connection.CreateModel();
                return channel;
            });

            return services;
        }
    }
}
