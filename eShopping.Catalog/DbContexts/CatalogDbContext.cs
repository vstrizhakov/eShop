using eShopping.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Catalog.DbContexts
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Announce> Announces { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Shop> Shops { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAutoscaleThroughput(1000);

            modelBuilder.Entity<Announce>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<Currency>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<Shop>()
                .HasPartitionKey(e => e.PartitionKey);
        }
    }
}
