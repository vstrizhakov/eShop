using eShop.Catalog.Entities;

namespace eShop.Catalog.Repositories
{
    public interface ICompositionRepository
    {
        Task<IEnumerable<Composition>> GetCompositionsAsync(Guid ownerId);
        Task<Composition?> GetCompositionByIdAsync(Guid id);
        Task CreateCompositionAsync(Composition composition);
        Task DeleteCompositionAsync(Composition composition);
        Task UpdateCompositionAsync(Composition composition);
    }
}
