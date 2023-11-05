using Telegram.Bot.Types;

namespace eShop.Telegram.Services
{
    public interface IRateLimiter
    {
        Task AcquireTimeslotAsync(ChatId chatId, bool isGroup);
    }
}