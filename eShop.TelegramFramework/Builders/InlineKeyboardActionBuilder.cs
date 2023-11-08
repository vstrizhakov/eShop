using eShop.Bots.Common;
using eShop.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    internal class InlineKeyboardActionBuilder : IInlineKeyboardButtonBuilder<InlineKeyboardAction>
    {
        private readonly IBotContextConverter _botContextConverter;

        public InlineKeyboardActionBuilder(IBotContextConverter botContextConverter)
        {
            _botContextConverter = botContextConverter;
        }

        public InlineKeyboardButton Build(InlineKeyboardAction element)
        {
            var button = new InlineKeyboardButton(element.Caption)
            {
                CallbackData = _botContextConverter.Serialize(element.Action, element.Arguments),
            };

            return button;
        }
    }
}
