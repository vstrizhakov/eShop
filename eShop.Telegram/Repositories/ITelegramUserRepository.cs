using eShop.Telegram.Entities;

namespace eShop.Telegram.Repositories
{
    public interface ITelegramUserRepository
    {
        Task<TelegramUser?> GetTelegramUserByIdAsync(Guid id);
        Task UpdateAccountIdAsync(TelegramUser telegramUser, Guid accountId);
    }
}
