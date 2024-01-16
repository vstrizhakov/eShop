using eShopping.Accounts.DbContexts;
using eShopping.Accounts.Entities;
using eShopping.Database.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Accounts.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountsDbContext _context;

        public AccountRepository(AccountsDbContext context)
        {
            _context = context;
        }

        public async Task CreateAccountAsync(Account account)
        {
            _context.Accounts.Add(account);

            await _context.SaveChangesAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(Guid id)
        {
            var account = await _context.Accounts
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.Id == id);
            return account;
        }

        public async Task<Account?> GetAccountByPhoneNumberAsync(string phoneNumber)
        {
            var account = await _context.Accounts
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.PhoneNumber.Contains(phoneNumber));
            return account;
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await _context.SaveChangesAsync();
        }
    }
}
