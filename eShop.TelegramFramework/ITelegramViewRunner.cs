namespace eShop.TelegramFramework
{
    public interface ITelegramViewRunner
    {
        Task RunAsync(params ITelegramView[] views);
    }
}
