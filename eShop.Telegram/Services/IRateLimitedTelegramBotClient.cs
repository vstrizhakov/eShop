using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.Services
{
    public interface IRateLimitedTelegramBotClient
    {
        Task<T> SendRequestAsync<T>(ChatId chatId, bool isGroup, Func<ITelegramBotClient, T> action);
    }
}
