using eShop.Catalog.Entities;

namespace eShop.Catalog.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(Guid ownerId);
        Task<Product?> GetProductByIdAsync(Guid id);
        Task CreateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
