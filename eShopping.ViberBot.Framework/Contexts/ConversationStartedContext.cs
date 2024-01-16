namespace eShopping.ViberBot.Framework.Contexts
{
    public sealed class ConversationStartedContext
    {
        public string UserId { get; }

        public ConversationStartedContext(Callback callback)
        {
            UserId = callback.User!.Id!;
        }
    }
}
