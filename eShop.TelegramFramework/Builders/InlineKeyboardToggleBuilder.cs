using eShop.Bots.Common;
using eShop.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    internal class InlineKeyboardToggleBuilder : IInlineKeyboardButtonBuilder<InlineKeyboardToggle>
    {
        private readonly IBotContextConverter _botContextConverter;

        public InlineKeyboardToggleBuilder(IBotContextConverter botContextConverter)
        {
            _botContextConverter = botContextConverter;
        }

        public InlineKeyboardButton Build(InlineKeyboardToggle element)
        {
            var currentValue = element.CurrentValue;
            var text = currentValue ? element.TrueCaption : element.FalseCaption;

            var args = new List<string>();
            args.Add(element.Action);
            args.AddRange(element.Arguments);
            args.Add((!currentValue).ToString());
            var button = new InlineKeyboardButton(text)
            {
                CallbackData = _botContextConverter.Serialize(args.ToArray()),
            };

            return button;
        }
    }
}
