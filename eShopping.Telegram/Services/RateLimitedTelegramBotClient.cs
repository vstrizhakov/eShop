using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShopping.Telegram.Services
{
    public class RateLimitedTelegramBotClient : IRateLimitedTelegramBotClient
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IRateLimiter _rateLimiter;

        public RateLimitedTelegramBotClient(ITelegramBotClient botClient, IRateLimiter rateLimiter)
        {
            _botClient = botClient;
            _rateLimiter = rateLimiter;
        }

        public async Task<T> SendRequestAsync<T>(ChatId chatId, bool isGroup, Func<ITelegramBotClient, T> action)
        {
            await _rateLimiter.AcquireTimeslotAsync(chatId, isGroup);

            var result = action(_botClient);
            return result;
        }
    }
}
