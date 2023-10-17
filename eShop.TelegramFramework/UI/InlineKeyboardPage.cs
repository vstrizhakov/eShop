namespace eShop.TelegramFramework.UI
{
    public class InlineKeyboardPage : IInlineKeyboardContainer
    {
        public string Action { get; }
        public IEnumerable<string> Arguments { get; }
        public InlineKeyboardGrid Grid { get; }
        public InlineKeyboardNavigation? Navigation { get; set; }
        public int? Index { get; set; }

        public InlineKeyboardPage(InlineKeyboardGrid grid, string action, params string[] arguments)
        {
            Grid = grid;
            Action = action;
            Arguments = arguments;
        }
    }
}
