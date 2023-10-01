using Telegram.Bot.Types;

namespace eShop.Telegram.Inner
{
    public interface ITelegramMiddleware
    {
        Task ProcessAsync(Update update);
    }
}
