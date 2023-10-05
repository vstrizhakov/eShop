namespace eShop.TelegramFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TextMessageAttribute : Attribute
    {
        public string? Command { get; set; }
        public string? Action { get; set; }
    }
}
