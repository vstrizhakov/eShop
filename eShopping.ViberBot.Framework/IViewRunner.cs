namespace eShopping.ViberBot.Framework
{
    public interface IViewRunner
    {
        Task<Message?> RunAsync(IViberView view);
        internal Task<Message?> RunAsync(IViberView view, ViberContext? context);
    }
}