using Microsoft.EntityFrameworkCore;

namespace eShopping.Database
{
    public interface IDataSeeding<TContext> where TContext : DbContext
    {
        Task SeedAsync(TContext context);
    }
}
