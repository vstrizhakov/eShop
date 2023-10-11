using Telegram.Bot.Types;

namespace eShop.TelegramFramework
{
    public interface IContextStore
    {
        Task<string?> GetActiveContextAsync(Update update);
        Task SetActiveContextAsync(Update update, string? activeContext);
    }
}
