using Microsoft.Extensions.DependencyInjection;

namespace eShop.TelegramFramework.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTelegramFramework(this IServiceCollection services)
        {
            services.AddScoped<ITelegramMiddleware, TelegramMiddleware>();

            return services;
        }
    }
}
