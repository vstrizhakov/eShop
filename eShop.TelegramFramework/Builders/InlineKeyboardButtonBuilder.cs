using eShop.TelegramFramework.UI;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    internal class InlineKeyboardButtonBuilder : IInlineKeyboardButtonBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public InlineKeyboardButtonBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public InlineKeyboardButton Build<T>(T element) where T : IInlineKeyboardElement
        {
            var buttonBuilder = _serviceProvider.GetRequiredService(typeof(IInlineKeyboardButtonBuilder<>).MakeGenericType(element.GetType()));
            var button = buttonBuilder.GetType().GetMethod("Build")!.Invoke(buttonBuilder, new object[] { element }) as InlineKeyboardButton;
            return button!;
        }
    }
}
