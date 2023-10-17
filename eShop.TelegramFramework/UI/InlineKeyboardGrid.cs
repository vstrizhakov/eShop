namespace eShop.TelegramFramework.UI
{
    public class InlineKeyboardGrid : IInlineKeyboardContainer
    {
        public IEnumerable<IInlineKeyboardElement> Items { get; }

        public InlineKeyboardGrid(IEnumerable<IInlineKeyboardElement> items)
        {
            Items = items;
        }

        internal int CalculateRows()
        {
            return Items.Count();
        }
    }
}
