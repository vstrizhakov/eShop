using eShop.Catalog.Entities;

namespace eShop.Catalog.Services
{
    public interface ICompositionService
    {
        Task<IEnumerable<Composition>> GetCompositionsAsync(Guid ownerId);
        Task<Composition?> GetCompositionAsync(Guid id);
        Task CreateCompositionAsync(Composition composition, IFormFile image);
        Task DeleteCompositionAsync(Composition composition);
    }
}
