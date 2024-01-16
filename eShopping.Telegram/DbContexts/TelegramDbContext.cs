using eShopping.Telegram.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Telegram.DbContexts
{
    public class TelegramDbContext : DbContext
    {
        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<TelegramChat> TelegramChats { get; set; }

        public TelegramDbContext(DbContextOptions<TelegramDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAutoscaleThroughput(1000);

            modelBuilder.Entity<TelegramUser>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<TelegramChat>()
                .HasPartitionKey(e => e.PartitionKey);
        }
    }
}
