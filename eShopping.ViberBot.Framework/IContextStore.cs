namespace eShopping.ViberBot.Framework
{
    public interface IContextStore
    {
        Task<string?> GetActiveContextAsync(Callback callback);
        Task SetActiveContextAsync(Callback callback, string? activeContext);
    }
}
