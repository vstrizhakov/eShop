using eShopping.Bots.Common;
using eShopping.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopping.TelegramFramework.Builders
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
