using eShop.Bots.Common;
using eShop.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    internal class InlineKeyboardSelectBuilder : IInlineKeyboardMarkupBuilder<InlineKeyboardSelect>
    {
        private readonly IBotContextConverter _botContextConverter;

        public InlineKeyboardSelectBuilder(IBotContextConverter botContextConverter)
        {
            _botContextConverter = botContextConverter;
        }

        public InlineKeyboardMarkup Build(InlineKeyboardSelect select)
        {
            var buttons = new List<IEnumerable<InlineKeyboardButton>>();

            foreach (var option in select.Options)
            {
                var button = Build(option, select.Action);

                buttons.Add(new[] { button });
            }

            var markup = new InlineKeyboardMarkup(buttons);
            return markup;
        }

        private InlineKeyboardButton Build(InlineKeyboardSelectOption option, string action)
        {
            var button = new InlineKeyboardButton(option.Caption)
            {
                CallbackData = _botContextConverter.Serialize(action, option.Data),
            };
            return button;
        }
    }
}
