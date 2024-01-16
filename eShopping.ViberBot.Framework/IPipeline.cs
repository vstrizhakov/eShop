namespace eShopping.ViberBot.Framework
{
    public interface IPipeline
    {
        Task<Message?> HandleAsync(Callback callback);
    }
}
