using eShop.Telegram.Entities;

namespace eShop.Telegram.Repositories
{
    public interface ITelegramChatRepository
    {
        Task<TelegramChat?> GetTelegramChatByExternalIdAsync(long externalId);
        Task CreateTelegramChatAsync(TelegramChat chat);
        Task UpdateTelegramChatAsync(TelegramChat telegramChat);
        Task<TelegramChat?> GetTelegramChatByIdAsync(Guid telegramChatId);
        Task<IEnumerable<TelegramChat>> GetTelegramChatsByIdsAsync(IEnumerable<Guid> ids);
    }
}
