using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace eShop.Messaging.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMessageHandler<TMessage, THandler>(this IServiceCollection services)
            where THandler : class, IMessageHandler<TMessage>
            where TMessage : notnull, IMessage
        {
            services.AddHostedService<MessagingManager>();
            services.AddSingleton<IConsumer, RabbitMqConsumer<TMessage>>();
            services.Configure<RabbitMqConsumerOptions<TMessage>>(options =>
            {
                var name = typeof(TMessage).Name;
                options.QueueName = name;
                options.RoutingKey = name;
            });
            services.AddScoped<IMessageHandler<TMessage>, THandler>();

            return services;
        }

        public static IServiceCollection AddRequestHandler<TRequest, TResponse, THandler>(this IServiceCollection services)
            where TRequest : notnull, IRequest<TResponse>
            where TResponse : notnull, IResponse
            where THandler : class, IRequestHandler<TRequest, TResponse>
        {
            services.AddHostedService<MessagingManager>();
            services.AddSingleton<IConsumer, RabbitMqConsumer<TRequest>>();
            services.Configure<RabbitMqConsumerOptions<TRequest>>(options =>
            {
                var name = typeof(TRequest).Name;
                options.QueueName = name;
                options.RoutingKey = name;
            });
            services.AddScoped<IRequestHandler<TRequest, TResponse>, THandler>();
            services.AddScoped<IMessageHandler<TRequest>, RequestHandler<TRequest, TResponse>>();

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
