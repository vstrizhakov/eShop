using Microsoft.EntityFrameworkCore;

namespace eShop.Database.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> WithPartitionKey<TEntity>(this IQueryable<TEntity> query, Guid partitionKey)
            where TEntity : class
        {
            return query.WithPartitionKey(partitionKey.ToString());
        }

        public static IQueryable<TEntity> WithDiscriminatorAsPartitionKey<TEntity>(this IQueryable<TEntity> query)
            where TEntity : class
        {
            return query.WithPartitionKey(typeof(TEntity).Name);
        }
    }
}
