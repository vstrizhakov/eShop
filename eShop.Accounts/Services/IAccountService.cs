using eShop.Accounts.Entities;

namespace eShop.Accounts.Services
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByPhoneNumberAsync(string phoneNumber);
        Task<Account> RegisterAccountAsync(string phoneNumber, Account account);
        Task LinkTelegramUserAsync(Account account, Guid telegramuserId);
        Task LinkViberUserAsync(Account account, Guid viberUserId);
        Task LinkIdentityUserAsync(Account account, string identityUserId);
        Task<Account?> GetAccountByIdAsync(Guid value);
    }
}