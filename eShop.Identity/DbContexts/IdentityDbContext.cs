using eShop.Database;
using eShop.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Identity.DbContexts
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasAutoscaleThroughput(1000);

            builder.Entity<User>()
                .HasPartitionKey(e => e.PartitionKey);
        }
    }
}
