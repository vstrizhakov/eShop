using Microsoft.EntityFrameworkCore;

namespace eShop.Database
{
    public interface IDataSeeding<TContext> where TContext : DbContext
    {
        Task SeedAsync(TContext context);
    }
}
