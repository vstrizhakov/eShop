using Microsoft.Extensions.DependencyInjection;

namespace eShopping.Bots.Common.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddBotContextConverter(this IServiceCollection services)
        {
            services.AddTransient<IBotContextConverter, BotContextConverter>();

            return services;
        }
    }
}
