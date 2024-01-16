using Microsoft.Extensions.DependencyInjection;

namespace eShopping.ViberBot.Framework.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddViberFramework<TContextStore>(this IServiceCollection services)
            where TContextStore : class, IContextStore
        {
            services.AddScoped<IPipeline, DefaultPipeline>();

            services.AddScoped<IContextStore, TContextStore>();
            services.AddScoped<IViewRunner, DefaultViewRunner>();
            services.AddScoped<ICallbackHandler, DefaultCallbackHandler>();

            return services;
        }
    }
}
