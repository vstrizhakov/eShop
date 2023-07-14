using eShop.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace eShop.Messaging.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMessageHandler<TMessage, THandler>(
            this IServiceCollection services) where THandler : class, IMessageHandler<TMessage>
        {
            services.AddRabbitMq();
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
            services.AddRabbitMq();
            services.AddSingleton<IProducer, RabbitMqProducer>();

            return services;
        }

        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(_ =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
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
