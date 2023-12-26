using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Database
{
    internal class DatabaseDeploymentBuilder<TContext> : IDatabaseDeploymentBuilder<TContext>
        where TContext : DbContext
    {
        private readonly IServiceCollection _services;

        public DatabaseDeploymentBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IDatabaseDeploymentBuilder<TContext> AddDataSeeding<TDataSeeding>()
            where TDataSeeding : class, IDataSeeding<TContext>
        {
            _services.AddScoped<IDataSeeding<TContext>, TDataSeeding>();

            return this;
        }
    }
}
