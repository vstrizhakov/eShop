using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.UI
{
    public class InlineKeyboardSelectOption
    {
        public string Caption { get; }
        public string Data { get; }

        public InlineKeyboardSelectOption(string caption, string data)
        {
            Caption = caption;
            Data = data;
        }
    }
}
