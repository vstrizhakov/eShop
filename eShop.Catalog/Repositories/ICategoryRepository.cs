using eShop.Catalog.Entities;

namespace eShop.Catalog.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(Guid ownerId);
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task CreateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
