using eShop.Accounts.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Accounts.DbContexts
{
    public class AccountsDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountsDbContext(DbContextOptions<AccountsDbContext> options) : base(options)
        {
        }
    }
}
