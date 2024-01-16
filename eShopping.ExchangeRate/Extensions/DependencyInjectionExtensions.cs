using eShopping.ExchangeRate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace eShopping.ExchangeRate.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddExchangeRateClient(this IServiceCollection services, Action<ExchangeRateClientOptions> configure)
        {
            services.Configure(configure);
            services.AddHttpClient("ExchangeRate")
                .AddTypedClient<IExchangeRateClient>((httpClient, serviceProvider) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<ExchangeRateClientOptions>>();
                    return new ExchangeRateClient(options.Value, httpClient);
                });

            return services;
        }
    }
}
