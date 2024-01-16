using eShopping.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopping.TelegramFramework.Builders
{
    internal class InlineKeyboardNavigationBuilder : IInlineKeyboardMarkupBuilder<InlineKeyboardNavigation>
    {
        private readonly IInlineKeyboardButtonBuilder<InlineKeyboardAction> _actionBuilder;

        public InlineKeyboardNavigationBuilder(IInlineKeyboardButtonBuilder<InlineKeyboardAction> actionBuilder)
        {
            _actionBuilder = actionBuilder;
        }

        public IEnumerable<IEnumerable<InlineKeyboardButton>> Build(InlineKeyboardNavigation control)
        {
            var buttons = new List<IEnumerable<InlineKeyboardButton>>();

            foreach (var action in control.Actions)
            {
                var button = _actionBuilder.Build(action);
                buttons.Add(new[] { button });
            }

            return buttons;
        }
    }
}
