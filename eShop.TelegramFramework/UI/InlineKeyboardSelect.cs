using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.UI
{
    public class InlineKeyboardSelect : IInlineKeyboardContainer
    {
        public IEnumerable<InlineKeyboardSelectOption> Options { get; }
        public string Action { get; }
        public InlineKeyboardAction? Navigation { get; set; }

        public InlineKeyboardSelect(string action,  IEnumerable<InlineKeyboardSelectOption> options)
        {
            Action = action;
            Options = options;
        }
    }
}
