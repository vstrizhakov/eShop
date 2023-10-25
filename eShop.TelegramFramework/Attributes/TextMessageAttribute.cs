namespace eShop.TelegramFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TextMessageAttribute : Attribute
    {
        public string? Command { get; set; }
        public string? Action { get; set; }
    }
}
