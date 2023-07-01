using eShop.Accounts.Entities;

namespace eShop.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByTelegramUserIdAsync(Guid telegramUserId);
        Task CreateAccountAsync(Account account);
        Task<Account?> GetAccountByViberUserIdAsync(Guid viberUserId);
    }
}
