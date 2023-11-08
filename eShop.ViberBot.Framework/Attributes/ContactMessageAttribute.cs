namespace eShop.ViberBot.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ContactMessageAttribute : Attribute
    {
        public string? Action { get; set; }
        public string? ActiveAction { get; set; }
    }
}
