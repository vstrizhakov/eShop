using eShopping.Accounts.Entities;

namespace eShopping.Accounts.Services
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByPhoneNumberAsync(string phoneNumber);
        Task<Account> RegisterAccountAsync(string phoneNumber, Account account);
        Task LinkTelegramUserAsync(Account account, Guid telegramuserId);
        Task LinkViberUserAsync(Account account, Guid viberUserId);
        Task LinkIdentityUserAsync(Account account, Guid identityUserId);
        Task<Account?> GetAccountByIdAsync(Guid value);
    }
}