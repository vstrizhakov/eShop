using Telegram.Bot.Types;

namespace eShopping.TelegramFramework
{
    public interface IContextStore
    {
        Task<string?> GetActiveContextAsync(Update update);
        Task SetActiveContextAsync(Update update, string? activeContext);
    }
}
