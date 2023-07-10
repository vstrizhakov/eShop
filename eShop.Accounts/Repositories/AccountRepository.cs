using eShop.Accounts.DbContexts;
using eShop.Accounts.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Accounts.Repositories
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
                .FindAsync(id);
            return account;
        }

        public async Task<Account?> GetAccountByPhoneNumberAsync(string phoneNumber)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(e => e.PhoneNumber.Contains(phoneNumber));
            return account;
        }

        public async Task<Account?> GetAccountByTelegramUserIdAsync(Guid telegramUserId)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(e => e.TelegramUserId == telegramUserId);
            return account;
        }

        public async Task<Account?> GetAccountByViberUserIdAsync(Guid viberUserId)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(e => e.ViberUserId == viberUserId);
            return account;
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await _context.SaveChangesAsync();
        }
    }
}
