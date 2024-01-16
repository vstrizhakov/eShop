using eShopping.Accounts.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Accounts.DbContexts
{
    public class AccountsDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountsDbContext(DbContextOptions<AccountsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAutoscaleThroughput(1000);

            modelBuilder.Entity<Account>()
                .HasPartitionKey(e => e.PartitionKey);
        }
    }
}
