using eShop.Catalog.DbContexts;
using eShop.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _context;

        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task CreateProductAsync(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products
                .FindAsync(id);

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(Guid ownerId)
        {
            var products = await _context.Products
                .Where(e => e.OwnerId == ownerId)
                .ToListAsync();

            return products;
        }
    }
}
