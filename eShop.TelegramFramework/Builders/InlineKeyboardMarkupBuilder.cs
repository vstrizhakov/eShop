using eShop.TelegramFramework.UI;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    internal class InlineKeyboardMarkupBuilder : IInlineKeyboardMarkupBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public InlineKeyboardMarkupBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public InlineKeyboardMarkup Build<T>(T control) where T : IInlineKeyboardContainer
        {
            var builder = _serviceProvider.GetRequiredService<IInlineKeyboardMarkupBuilder<T>>();
            var markup = builder.Build(control);
            return markup;
        }
    }
}
