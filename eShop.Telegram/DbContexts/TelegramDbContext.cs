using eShop.Telegram.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Telegram.DbContexts
{
    public class TelegramDbContext : DbContext
    {
        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<TelegramChat> TelegramChats { get; set; }
        public DbSet<TelegramChatMember> TelegramChatMembers { get; set; }
        public DbSet<TelegramChatSettings> TelegramChatSettings { get; set; }

        public TelegramDbContext(DbContextOptions<TelegramDbContext> options) : base(options)
        {
        }
    }
}
