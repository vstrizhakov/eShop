using eShop.Viber.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Viber.DbContexts
{
    public class ViberDbContext : DbContext
    {
        public DbSet<ViberUser> ViberUsers { get; set; }
        public DbSet<ViberChatSettings> ViberChatSettings { get; set; }

        public ViberDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
