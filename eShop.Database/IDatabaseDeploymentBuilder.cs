using Microsoft.EntityFrameworkCore;

namespace eShop.Database
{
    public interface IDatabaseDeploymentBuilder<TContext>
        where TContext : DbContext
    {
        IDatabaseDeploymentBuilder<TContext> AddDataSeeding<TDataSeeding>()
            where TDataSeeding : class, IDataSeeding<TContext>;
    }
}
