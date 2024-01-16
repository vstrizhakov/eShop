using Telegram.Bot.Types;

namespace eShopping.TelegramFramework
{
    public interface ITelegramMiddleware
    {
        Task HandleUpdateAsync(Update update);
    }
}
