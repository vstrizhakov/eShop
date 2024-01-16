namespace eShopping.TelegramFramework
{
    public interface ITelegramViewRunner
    {
        Task RunAsync(params ITelegramView[] views);
    }
}
