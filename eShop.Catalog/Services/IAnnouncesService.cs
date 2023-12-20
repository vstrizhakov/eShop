using eShop.Catalog.Entities;

namespace eShop.Catalog.Services
{
    public interface IAnnouncesService
    {
        Task<IEnumerable<Announce>> GetAnnouncesAsync(Guid ownerId);
        Task<Announce?> GetAnnounceAsync(Guid id, Guid ownerId);
        Task CreateAnnounceAsync(Announce announce, IFormFile image);
        Task DeleteAnnounceAsync(Announce announce);
        Task SetAnnounceDistributionIdAsync(Announce announce, Guid distributionId);
    }
}
