using eShop.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    internal class InlineKeyboardGridBuilder : IInlineKeyboardMarkupBuilder<InlineKeyboardGrid>
    {
        private const int MaxRowsCount = 3;
        private const int MaxColumnsCount = 10;

        private readonly IInlineKeyboardButtonBuilder _buttonBuilder;

        public InlineKeyboardGridBuilder(IInlineKeyboardButtonBuilder buttonBuilder)
        {
            _buttonBuilder = buttonBuilder;
        }

        public IEnumerable<IEnumerable<InlineKeyboardButton>> Build(InlineKeyboardGrid control)
        {
            var buttons = new List<IEnumerable<InlineKeyboardButton>>();

            foreach (var item in control.Items)
            {
                var button = _buttonBuilder.Build(item);

                buttons.Add(new[] { button });
            }

            return buttons;
        }
    }
}
