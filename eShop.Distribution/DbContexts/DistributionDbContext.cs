using eShop.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.DbContexts
{
    public class DistributionDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TelegramChat> TelegramChats { get; set; }
        public DbSet<ViberChat> ViberChats { get; set; }
        public DbSet<DistributionGroup> DistributionGroups { get; set; }
        public DbSet<DistributionGroupItem> DistributionGroupItems { get; set; }

        public DistributionDbContext(DbContextOptions<DistributionDbContext> options) : base(options)
        {
        }
    }
}
