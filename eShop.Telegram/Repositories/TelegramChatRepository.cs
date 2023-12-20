using eShop.Database.Extensions;
using eShop.Telegram.DbContexts;
using eShop.Telegram.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Telegram.Repositories
{
    public class TelegramChatRepository : ITelegramChatRepository
    {
        private readonly TelegramDbContext _context;

        public TelegramChatRepository(TelegramDbContext context)
        {
            _context = context;
        }

        public async Task CreateTelegramChatAsync(TelegramChat chat)
        {
            _context.TelegramChats.Add(chat);

            await _context.SaveChangesAsync();
        }

        public async Task<TelegramChat?> GetTelegramChatByExternalIdAsync(long externalId)
        {
            var telegramChat = await _context.TelegramChats
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.ExternalId == externalId);
            return telegramChat;
        }

        public async Task<TelegramChat?> GetTelegramChatByIdAsync(Guid id)
        {
            var telegramChat = await _context.TelegramChats
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.Id == id);
            return telegramChat;
        }

        public async Task<IEnumerable<TelegramChat>> GetTelegramChatsByIdsAsync(IEnumerable<Guid> ids)
        {
            var telegramChats = await _context.TelegramChats
                .WithDiscriminatorAsPartitionKey()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
            return telegramChats;
        }

        public async Task UpdateTelegramChatAsync(TelegramChat telegramChat)
        {
            await _context.SaveChangesAsync();
        }
    }
}
