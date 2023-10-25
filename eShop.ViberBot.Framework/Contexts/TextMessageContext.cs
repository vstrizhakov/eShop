namespace eShop.ViberBot.Framework.Contexts
{
    public sealed class TextMessageContext
    {
        public string UserId { get; }
        public string Text { get; }

        public TextMessageContext(Callback callback)
        {
            UserId = callback.Sender!.Id!;
            Text = callback.Message!.Text!;
        }
    }
}
