using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task CreateAccountAsync(Account account);
        Task UpdateTelegramChatAsync(Account account, Guid telegramChatId, bool isEnabled);
        Task UpdateViberChatAsync(Account account, Guid viberUserId, bool isEnabled);
    }
}
