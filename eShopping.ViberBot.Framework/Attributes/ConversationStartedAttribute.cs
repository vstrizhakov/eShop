namespace eShopping.ViberBot.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ConversationStartedAttribute : Attribute
    {
        public string? Action { get; set; }
    }
}
