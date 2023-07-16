using eShop.Accounts.Entities;

namespace eShop.Accounts.Services
{
    public interface IAccountService
    {
        Task<Account> RegisterAccountByTelegramUserIdAsync(Guid providerId, Account account);
        Task<Account> RegisterAccountByViberUserIdAsync(Guid providerId, Account account);
    }
}