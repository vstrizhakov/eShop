using eShopping.TelegramFramework.UI;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopping.TelegramFramework.Builders
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
            var buttons = builder.Build(control);
            return new InlineKeyboardMarkup(buttons);
        }
    }
}
