namespace eShopping.TelegramFramework.UI
{
    public class InlineKeyboardPagination : IInlineKeyboardContainer
    {
        public InlineKeyboardPage Page { get; set; }

        public InlineKeyboardPagination(InlineKeyboardPage page)
        {
            Page = page;
        }

        public int CalculateRows()
        {
            var MaxRows = 10;
            var PaginationRows = 1;

            var rows = 0;
            if (Page.Navigation != null)
            {
                rows += Page.Navigation.CalculateRows();
            }

            var availableRows = MaxRows - rows;
            if (Page.Grid.CalculateRows() > availableRows)
            {
                rows += PaginationRows;
            }

            return MaxRows - rows;
        }
    }
}
