namespace eShopping.ViberBot.Framework
{
    internal interface ICallbackHandler
    {
        Task<Message?> HandleAsync(Callback callback);
    }
}
