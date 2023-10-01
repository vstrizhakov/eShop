using eShop.Telegram.Entities;

namespace eShop.Telegram.Services
{
    public interface ITelegramService
    {
        Task<TelegramUser?> GetUserByExternalIdAsync(long externalId);
        IEnumerable<TelegramChatMember> GetManagableChats(TelegramUser user);
        Task UpdateUser(TelegramUser user);
        Task<TelegramUser?> GetUserByAccountIdAsync(Guid accountId);
    }
}
