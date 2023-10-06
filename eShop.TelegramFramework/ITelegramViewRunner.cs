namespace eShop.TelegramFramework
{
    public interface ITelegramViewRunner
    {
        Task RunAsync(ITelegramView view);
    }
}
