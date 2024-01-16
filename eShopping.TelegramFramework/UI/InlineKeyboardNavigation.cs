namespace eShopping.TelegramFramework.UI
{
    public class InlineKeyboardNavigation : IInlineKeyboardContainer
    {
        public IEnumerable<InlineKeyboardAction> Actions { get; set; }

        public InlineKeyboardNavigation(IEnumerable<InlineKeyboardAction> actions)
        {
            Actions = actions;
        }

        public InlineKeyboardNavigation(InlineKeyboardAction action) : this(new[] { action })
        {
        }

        internal int CalculateRows()
        {
            return 1;
        }
    }
}
