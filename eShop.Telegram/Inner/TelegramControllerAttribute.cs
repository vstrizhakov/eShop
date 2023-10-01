namespace eShop.Telegram.Inner
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TelegramControllerAttribute : Attribute
    {
        public TelegramContext Context { get; set; }
        public string? Command { get; set; }
        public bool? IsDefault { get; set; }
        public string? Action { get; set; }

        public TelegramControllerAttribute()
        {
        }

        public TelegramControllerAttribute(string action)
        {
            Action = action;
        }
    }
}
