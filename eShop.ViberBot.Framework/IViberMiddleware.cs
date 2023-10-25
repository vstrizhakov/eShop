namespace eShop.ViberBot.Framework
{
    public interface IViberMiddleware
    {
        Task ProcessAsync(Callback callback);
    }
}
