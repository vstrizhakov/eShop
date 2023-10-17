using eShop.Bots.Common;
using eShop.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    internal class InlineKeyboardPaginationBuilder : IInlineKeyboardMarkupBuilder<InlineKeyboardPagination>
    {
        private readonly IBotContextConverter _botContextConverter;

        public InlineKeyboardPaginationBuilder(IBotContextConverter botContextConverter)
        { 
            _botContextConverter = botContextConverter;
        }

        public IEnumerable<IEnumerable<InlineKeyboardButton>> Build(InlineKeyboardPagination control)
        {
            var page = control.Page;

            var availableRows = control.CalculateRows();
            var totalItemsCount = page.Grid.CalculateRows();

            var buttons = new List<IEnumerable<InlineKeyboardButton>>();

            var line = new List<InlineKeyboardButton>();
            var pagesCount = (int)Math.Floor((double)totalItemsCount / availableRows);

            var MaxColumnsCount = 10;
            var addArrows = pagesCount > MaxColumnsCount;
            var pagesToDisplay = addArrows ? pagesCount - 2 : pagesCount;

            if (addArrows)
            {
                var button = new InlineKeyboardButton("<")
                {
                    CallbackData = "None",
                };

                line.Add(button);
            }

            for (int i = 0; i < pagesToDisplay; i++)
            {
                var button = new InlineKeyboardButton((i + 1).ToString())
                {
                    CallbackData = _botContextConverter.Serialize(page.Action, i.ToString()),
                };

                line.Add(button);
            }

            if (addArrows)
            {
                var button = new InlineKeyboardButton(">")
                {
                    CallbackData = "None",
                };

                line.Add(button);
            }

            buttons.Add(line);

            return buttons;
        }
    }
}
