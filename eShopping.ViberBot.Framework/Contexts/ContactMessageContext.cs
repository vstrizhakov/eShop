namespace eShopping.ViberBot.Framework.Contexts
{
    public sealed class ContactMessageContext
    {
        public string UserId { get; }
        public Contact Contact { get; }

        public ContactMessageContext(Callback callback)
        {
            UserId = callback.Sender!.Id!;
            Contact = callback.Message!.Contact!;
        }
    }
}
