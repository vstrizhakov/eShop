using eShop.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.DbContexts
{
    public class DistributionDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<DefaultCurrencyRate> DefaultCurrencyRates { get; set; }
        public DbSet<Entities.Distribution> Distribution { get; set; }
        public DbSet<Shop> Shops { get; set; }

        public DistributionDbContext(DbContextOptions<DistributionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAutoscaleThroughput(1000);

            modelBuilder.Entity<Account>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<Currency>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<Entities.Distribution>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<DefaultCurrencyRate>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<Shop>()
                .HasPartitionKey(e => e.PartitionKey);
        }
    }
}
