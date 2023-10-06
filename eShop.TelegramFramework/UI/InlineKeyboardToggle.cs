namespace eShop.TelegramFramework.UI
{
    public class InlineKeyboardToggle : IInlineKeyboardElement
    {
        public string FalseCaption { get; }
        public string TrueCaption { get; }
        public bool CurrentValue { get; }
        public string Action { get; }
        public string[] Arguments { get; }

        public InlineKeyboardToggle(string falseCaption, string trueCaption, bool currentValue, string action, params string[] arguments)
        {
            Action = action;
            CurrentValue = currentValue;
            FalseCaption = falseCaption;
            TrueCaption = trueCaption;
            Arguments = arguments;
        }
    }
}
