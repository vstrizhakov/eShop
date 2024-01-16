using eShopping.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopping.TelegramFramework.Builders
{
    internal class InlineKeyboardPageBuilder : IInlineKeyboardMarkupBuilder<InlineKeyboardPage>
    {
        private readonly IInlineKeyboardMarkupBuilder<InlineKeyboardGrid> _gridBuilder;
        private readonly IInlineKeyboardMarkupBuilder<InlineKeyboardPagination> _paginationBuilder;
        private readonly IInlineKeyboardMarkupBuilder<InlineKeyboardNavigation> _navigationBuilder;

        public InlineKeyboardPageBuilder(
            IInlineKeyboardMarkupBuilder<InlineKeyboardGrid> gridBuilder,
            IInlineKeyboardMarkupBuilder<InlineKeyboardPagination> paginationBuilder,
            IInlineKeyboardMarkupBuilder<InlineKeyboardNavigation> navigationBuilder)
        {
            _gridBuilder = gridBuilder;
            _paginationBuilder = paginationBuilder;
            _navigationBuilder = navigationBuilder;
        }

        public IEnumerable<IEnumerable<InlineKeyboardButton>> Build(InlineKeyboardPage page)
        {
            var pagination = new InlineKeyboardPagination(page);
            var availableRows = pagination.CalculateRows();

            var buttons = new List<IEnumerable<InlineKeyboardButton>>();

            var pageIndex = page.Index ?? 0;
            var gridLines = _gridBuilder.Build(page.Grid);
            buttons.AddRange(gridLines.Skip(availableRows * pageIndex).Take(availableRows));

            if (gridLines.Count() > availableRows)
            {
                var paginationLines = _paginationBuilder.Build(pagination);
                buttons.AddRange(paginationLines);
            }

            var navigation = page.Navigation;
            if (navigation != null)
            {
                var navigationLines = _navigationBuilder.Build(navigation);
                buttons.AddRange(navigationLines);
            }

            return buttons;
        }
    }
}
