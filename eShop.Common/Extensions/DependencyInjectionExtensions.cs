using Microsoft.Extensions.DependencyInjection;

namespace eShop.Common.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddPublicUriBuilder(this IServiceCollection services, Action<PublicUriConfiguration> configure)
        {
            services.Configure(configure);
            services.AddTransient<IPublicUriBuilder, PublicUriBuilder>();

            return services;
        }
    }
}
