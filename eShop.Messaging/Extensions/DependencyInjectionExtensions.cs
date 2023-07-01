using eShop.RabbitMq.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Messaging.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddRabbitMqMessageHandler(this IServiceCollection services)
        {
            services.AddRabbitMqConsumer<RabbitMqMessageHandler>();
            return services;
        }
    }
}
