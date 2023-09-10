using eShop.Distribution.DbContexts;
using eShop.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DistributionDbContext _context;

        public AccountRepository(DistributionDbContext context)
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
                .Include(e => e.TelegramChats)
                .Include(e => e.ViberChat)
                .FirstOrDefaultAsync(e => e.Id == id);
            return account;
        }

        public async Task<IEnumerable<Account>> GetAccountsByProviderIdAsync(Guid providerId)
        {
            var accounts = await _context.Accounts
                .Include(e => e.TelegramChats)
                .Include(e => e.ViberChat)
                .Where(e => e.ProviderId == providerId)
                .ToListAsync();
            return accounts;
        }

        public async Task UpdateTelegramChatAsync(Account account, Guid telegramChatId, bool isEnabled)
        {
            var telegramChats = account.TelegramChats;
            var telegramChat = telegramChats.FirstOrDefault(e => e.Id == telegramChatId);
            if (telegramChat == null)
            {
                telegramChat = new TelegramChat
                {
                    Id = telegramChatId,
                };

                var a = _context.Entry(telegramChat).State;

                telegramChats.Add(telegramChat);


                var b = _context.Entry(telegramChat).State;
            }

            telegramChat.IsEnabled = isEnabled;

            var c = _context.Entry(telegramChat).State;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateViberChatAsync(Account account, Guid viberUserId, bool isEnabled)
        {
            var viberChat = account.ViberChat;
            if (viberChat == null)
            {
                viberChat = new ViberChat
                {
                    Id = viberUserId,
                };

                account.ViberChat = viberChat;
            }

            viberChat.IsEnabled = isEnabled;

            await _context.SaveChangesAsync();
        }
    }
}
