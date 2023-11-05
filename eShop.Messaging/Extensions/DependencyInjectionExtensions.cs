using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace eShop.Messaging.Extensions
{
    public static class DependencyInjectionExtensions
    {
        private static IServiceCollection AddBasicMessaging(this IServiceCollection services)
        {
            services.AddHostedService<MessagingManager>();
            services.AddScoped(typeof(IMessagePipeline<>), typeof(MessagePipeline<>));
            services.AddScoped<IRequestClient, RequestClient>();

            return services;
        }

        public static IServiceCollection AddMessageListener<TMessage>(this IServiceCollection services, Action<RabbitMqConsumerOptions<TMessage>>? configure = null)
            where TMessage : notnull, IMessage
        {
            services.AddBasicMessaging();

            services.AddSingleton<MessageBridge<TMessage>>();
            services.AddSingleton<IMessagePublisher<TMessage>>(serviceProvider => serviceProvider.GetRequiredService<MessageBridge<TMessage>>());
            services.AddSingleton<IMessageObserver<TMessage>>(serviceProvider => serviceProvider.GetRequiredService<MessageBridge<TMessage>>());

            services.AddSingleton<IConsumer, RabbitMqConsumer<TMessage>>();
            services.Configure<RabbitMqConsumerOptions<TMessage>>(options =>
            {
                var name = typeof(TMessage).Name;
                options.QueueName = name;
                options.RoutingKey = name;

                if (configure != null)
                {
                    configure(options);
                }
            });

            return services;
        }

        public static IServiceCollection AddMessageHandler<TMessage, THandler>(this IServiceCollection services, Action<RabbitMqConsumerOptions<TMessage>>? configure = null)
            where THandler : class, IMessageHandler<TMessage>
            where TMessage : notnull, IMessage
        {
            services.AddMessageListener<TMessage>(configure);
            services.AddScoped<IMessageHandler<TMessage>, THandler>();

            return services;
        }

        public static IServiceCollection AddRequestHandler<TRequest, TResponse, THandler>(this IServiceCollection services)
            where TRequest : notnull, IRequest<TResponse>
            where TResponse : notnull, IResponse
            where THandler : class, IRequestHandler<TRequest, TResponse>
        {
            services.AddScoped<IRequestHandler<TRequest, TResponse>, THandler>();
            services.AddMessageHandler<TRequest, RequestHandler<TRequest, TResponse>>();

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
