namespace eShopping.ViberBot.Framework
{
    public interface IViberMiddleware
    {
        Task ProcessAsync(Callback callback);
    }
}
