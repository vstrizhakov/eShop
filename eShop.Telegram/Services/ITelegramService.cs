using eShop.Telegram.Entities;

namespace eShop.Telegram.Services
{
    public interface ITelegramService
    {
        Task<TelegramUser?> GetUserByExternalIdAsync(long externalId);
        IEnumerable<TelegramChatMember> GetManagableChats(TelegramUser user);
        Task UpdateUser(TelegramUser user);
        Task<TelegramUser?> GetUserByAccountIdAsync(Guid accountId);
        Task<TelegramUser?> GetUserByTelegramUserIdAsync(Guid telegramUserId);
        Task SetAccountIdAsync(TelegramUser user, Guid accountId);
        Task SetActiveContextAsync(TelegramUser user, string? context);
    }
}
