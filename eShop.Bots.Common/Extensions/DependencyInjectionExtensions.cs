using eShop.Services;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Bots.Common.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddBotContextConverter(this IServiceCollection services)
        {
            services.AddTransient<IBotContextConverter, BotContextConverter>();

            return services;
        }

        public static IServiceCollection AddPublicUriBuilder(this IServiceCollection services, Action<PublicUriConfiguration> configure)
        {
            services.Configure(configure);
            services.AddTransient<IPublicUriBuilder, PublicUriBuilder>();

            return services;
        }
    }
}
