using eShop.Accounts.Entities;

namespace eShop.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByTelegramUserIdAsync(Guid telegramUserId);
        Task<Account?> GetAccountByPhoneNumberAsync(string phoneNumber);
        Task CreateAccountAsync(Account account);
        Task<Account?> GetAccountByViberUserIdAsync(Guid viberUserId);
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task UpdateAccountAsync(Account account);
        Task<Account?> GetAccountByIdentityUserIdAsync(string identityUserId);
    }
}
