using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.TelegramFramework.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTelegramFramework(this IServiceCollection services)
        {
            services.AddScoped<ITelegramMiddleware, TelegramMiddleware>();

            services.AddScoped<ITelegramViewRunner , TelegramViewRunner>();

            services.AddScoped<IInlineKeyboardMarkupBuilder, InlineKeyboardMarkupBuilder>();
            services.AddScoped<IInlineKeyboardMarkupBuilder<InlineKeyboardSelect>, InlineKeyboardSelectBuilder>();
            services.AddScoped<IInlineKeyboardMarkupBuilder<InlineKeyboardList>, InlineKeyboardListBuilder>();

            services.AddScoped<IInlineKeyboardButtonBuilder, InlineKeyboardButtonBuilder>();
            services.AddScoped<IInlineKeyboardButtonBuilder<InlineKeyboardToggle>, InlineKeyboardToggleBuilder>();
            services.AddScoped<IInlineKeyboardButtonBuilder<InlineKeyboardAction>, InlineKeyboardActionBuilder>();

            return services;
        }
    }
}
