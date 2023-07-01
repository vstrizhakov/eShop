using eShop.Telegram.DbContexts;
using eShop.Telegram.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Telegram.Repositories
{
    public class TelegramUserRepository : ITelegramUserRepository
    {
        private readonly TelegramDbContext _context;

        public TelegramUserRepository(TelegramDbContext context)
        {
            _context = context;
        }

        public Task<TelegramUser?> GetTelegramUserByIdAsync(Guid id)
        {
            var telegramUser = _context.TelegramUsers
                .Include(e => e.Chats)
                    .ThenInclude(e => e.Chat)
                .FirstOrDefaultAsync(e => e.Id == id);
            return telegramUser;
        }

        public async Task UpdateAccountIdAsync(TelegramUser telegramUser, Guid accountId)
        {
            telegramUser.AccountId = accountId;

            await _context.SaveChangesAsync();
        }
    }
}
