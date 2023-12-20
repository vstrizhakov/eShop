using eShop.Viber.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Viber.DbContexts
{
    public class ViberDbContext : DbContext
    {
        public DbSet<ViberUser> ViberUsers { get; set; }

        public ViberDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAutoscaleThroughput(1000);

            modelBuilder.Entity<ViberUser>()
                .HasPartitionKey(e => e.PartitionKey);
        }
    }
}
