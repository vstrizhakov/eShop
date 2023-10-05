using Telegram.Bot.Types;

namespace eShop.TelegramFramework
{
    public interface ITelegramMiddleware
    {
        Task ProcessAsync(Update update, string? activeContext = null);
    }
}
