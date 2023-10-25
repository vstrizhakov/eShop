namespace eShop.TelegramFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CallbackQueryAttribute : Attribute
    {
        public string Action { get; set; }

        public CallbackQueryAttribute(string action)
        {
            Action = action;
        }
    }
}
