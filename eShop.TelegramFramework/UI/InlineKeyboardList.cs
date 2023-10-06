namespace eShop.TelegramFramework.UI
{
    public class InlineKeyboardList : IInlineKeyboardContainer
    {
        public IEnumerable<IInlineKeyboardElement> Items { get; }
        public InlineKeyboardAction? Navigation { get; set; }

        public InlineKeyboardList(IEnumerable<IInlineKeyboardElement> items)
        {
            Items = items;
        }
    }
}
