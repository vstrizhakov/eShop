namespace eShop.ViberBot.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TextMessageAttribute : Attribute
    {
        public string? Action { get; set; }
        public string? ActiveAction { get; set; }
    }
}
