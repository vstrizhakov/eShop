using eShop.Catalog.DbContexts;
using eShop.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogDbContext _context;

        public CategoryRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(Guid ownerId)
        {
            var categories = await _context.Categories
                .Where(e => e.OwnerId == ownerId)
                .ToListAsync();

            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories
                .FindAsync(id);

            return category;
        }
    }
}
