namespace eShopping.TelegramFramework.UI
{
    public record InlineKeyboardAction(string Caption, string Action, params string[] Arguments) : IInlineKeyboardElement;
}
