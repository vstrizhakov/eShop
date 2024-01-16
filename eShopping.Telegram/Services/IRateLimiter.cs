using Telegram.Bot.Types;

namespace eShopping.Telegram.Services
{
    public interface IRateLimiter
    {
        Task AcquireTimeslotAsync(ChatId chatId, bool isGroup);
    }
}