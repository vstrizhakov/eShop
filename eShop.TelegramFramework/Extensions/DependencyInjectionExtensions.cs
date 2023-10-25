using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.TelegramFramework.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTelegramFramework<TContextStore>(this IServiceCollection services)
            where TContextStore : class, IContextStore
        {
            var updateBridge = new UpdateBridge();
            services.AddSingleton<IUpdatePublisher>(updateBridge);
            services.AddSingleton<IUpdateObserver>(updateBridge);

            services.AddScoped<IUpdatePipeline, UpdatePipeline>();

            services.AddHostedService<BackgroundService>();

            services.AddScoped<IContextStore, TContextStore>();
            services.AddScoped<ITelegramViewRunner , TelegramViewRunner>();
            services.AddScoped<ITelegramMiddleware, RoutingMiddleware>();

            services.AddScoped<IInlineKeyboardMarkupBuilder, InlineKeyboardMarkupBuilder>();
            services.AddScoped<IInlineKeyboardMarkupBuilder<InlineKeyboardPage>, InlineKeyboardPageBuilder>();
            services.AddScoped<IInlineKeyboardMarkupBuilder<InlineKeyboardPagination>, InlineKeyboardPaginationBuilder>();
            services.AddScoped<IInlineKeyboardMarkupBuilder<InlineKeyboardGrid>, InlineKeyboardGridBuilder>();
            services.AddScoped<IInlineKeyboardMarkupBuilder<InlineKeyboardNavigation>, InlineKeyboardNavigationBuilder>();

            services.AddScoped<IInlineKeyboardButtonBuilder, InlineKeyboardButtonBuilder>();
            services.AddScoped<IInlineKeyboardButtonBuilder<InlineKeyboardToggle>, InlineKeyboardToggleBuilder>();
            services.AddScoped<IInlineKeyboardButtonBuilder<InlineKeyboardAction>, InlineKeyboardActionBuilder>();

            return services;
        }
    }
}
