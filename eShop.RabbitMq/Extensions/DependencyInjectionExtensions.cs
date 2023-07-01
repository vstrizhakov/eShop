using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace eShop.RabbitMq.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddRabbitMqProducer(this IServiceCollection services)
        {
            services.AddRabbitMq();
            services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();

            return services;
        }

        public static IServiceCollection AddRabbitMqConsumer<T>(this IServiceCollection services) where T : class, IRabbitMqMessageHandler
        {
            services.AddRabbitMq();
            services.AddSingleton<IRabbitMqMessageHandler, T>();
            services.AddHostedService<RabbitMqConsumer>();

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
