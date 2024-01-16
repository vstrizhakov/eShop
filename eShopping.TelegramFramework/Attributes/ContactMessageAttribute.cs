namespace eShopping.TelegramFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ContactMessageAttribute : Attribute
    {
        public string? Action { get; set; }
    }
}
