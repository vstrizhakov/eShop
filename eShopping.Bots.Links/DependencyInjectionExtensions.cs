using eShopping.Bots.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace eShopping.Bots.Links
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddViberLinks(this IServiceCollection services, Action<ViberLinkOptions> configure)
        {
            services.AddBotContextConverter();

            services.Configure(configure);
            services.AddTransient<IViberLinkGenerator, ViberLinkGenerator>();

            return services;
        }

        public static IServiceCollection AddTelegramLinks(this IServiceCollection services, Action<TelegramLinkOptions> configure)
        {
            services.AddBotContextConverter();

            services.Configure(configure);
            services.AddTransient<ITelegramLinkGenerator, TelegramLinkGenerator>();

            return services;
        }
    }
}
