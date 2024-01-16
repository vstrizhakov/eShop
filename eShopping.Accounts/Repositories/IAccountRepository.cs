using eShopping.Accounts.Entities;

namespace eShopping.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByPhoneNumberAsync(string phoneNumber);
        Task CreateAccountAsync(Account account);
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task UpdateAccountAsync(Account account);
    }
}
